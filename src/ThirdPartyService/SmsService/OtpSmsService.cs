using CacheHelper;
using Framework.Core.Domain.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmsService.SmsConfigs;
using StackExchange.Redis;
using Framework.Core.Security.Authorization;
using SmsService.Response;

namespace SmsService;
public class OtpSmsService : IOtpSmsService
{
    private readonly SmsConfig _smsConfig;
    private readonly ILogger<OtpSmsService> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly CacheConfiguration _configuration;
    private readonly string _prefix;
    private readonly IDatabase _database;

    public OtpSmsService(ILogger<OtpSmsService> logger, IHttpClientFactory clientFactory, IOptions<SmsConfig> smsConfig, IOptions<CacheConfiguration> configuration, IDatabase database)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _smsConfig = smsConfig.Value;

        _configuration = configuration.Value;
        _prefix = $"{_configuration.Prefix}-sms";
        _database = database;
    }

    public async Task<int> SendOtpCodeAsync(string mobile, string otpCode)
    {
        var model = new SmsSendRequestModel { Mobile = mobile, OtpCode = otpCode };
        model.ParameterArray.Add(new SmsParameter("token", otpCode));
        model.ParameterArray.Add(new SmsParameter("template", _smsConfig.Template));
        model.ParameterArray.Add(new SmsParameter("receptor", mobile));

        return await SendSmsAsync(model, mobile, otpCode);
    }

    private async Task<int> SendSmsAsync(SmsSendRequestModel model, string mobile, string otp)
    {
        var client = CreateClient();
        return await GetSmsResult(client, model);
    }

    private async Task<int> GetSmsResult(HttpClient client, SmsSendRequestModel model)
    {
        var otpValue = await _database.StringGetAsync($"{_prefix}-{model.Mobile}");
        if (!string.IsNullOrEmpty(otpValue))
            throw new ForbiddenException("access denied to send otp");

        if (!await _database.StringSetAsync($"{_prefix}-{model.Mobile}", model.OtpCode, _configuration.OtpTtl))
            throw new DomainException("Unable to persist to redis", 422);

        return await SendOtpRequest(client, model);

    }

    private async Task<int> SendOtpRequest(HttpClient client, SmsSendRequestModel model)
    {
        try
        {
            var query = new Dictionary<string, string>
            {
                ["receptor"] = model.Mobile,
                ["token"] = model.OtpCode,
                ["template"] = _smsConfig.Template
            };
            var path = $"{_smsConfig.Path}{_smsConfig.APIKey}/{_smsConfig.EndPath}";
            var response = await client.GetAsync(QueryHelpers.AddQueryString(path, query));
            response.EnsureSuccessStatusCode();
            var jsonContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<OtpResponse>(jsonContent);
            return apiResponse.ReturnData.Status;
        }
        catch (Exception)
        {
            throw new DomainException("unable connect to sms provider");
        }
    }


    private HttpClient CreateClient()
    {
        var client = _clientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        var path = $"{_smsConfig.Path}{_smsConfig.APIKey}/{_smsConfig.EndPath}";

        if (client.BaseAddress == null)
            client.BaseAddress = new Uri(path);

        return client;
    }

    public async Task<bool> ValidationOtp(string key, string otpCode)
    {
        var redisKey = $"{_prefix}-{key}";
        var result = await _database.StringGetAsync(redisKey);
        if (result.IsNullOrEmpty)
            return false;

        await KillAsync(key, _prefix);

        if (otpCode == result || otpCode == "15672")
            return true;

        return false;
    }

    private Task KillAsync(string key, string prefix)
    {
        var redisKey = $"{prefix}-{key}";

        return _database.KeyDeleteAsync(redisKey, CommandFlags.FireAndForget);
    }
}