using Framework.Core.Security.Authorization;
using Framework.Core.Security.Token;
using Framework.Web.Api.Controllers;
using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OFood.Shop.Api.Configurations;
using OFood.Shop.Api.Controllers.V1.Authenticate.Models;
using OFood.Shop.Application.Contract.Customers.Command.CommandRequest;
using OFood.Shop.Facade.Customers;
using SmsService;
using UserManagement.Security;

namespace OFood.Shop.Api.Controllers.V1.Authenticate;

[ApiController]
[Route("app-api/authenticate")]
public class AuthenticateController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IOtpSmsService _otpSmsService;
    private readonly OtpConfig _otpConfig;
    private readonly TokenExpirationConfig _tokenExpirationConfig;
    private readonly JwtBearerTokenSettings _jwt;
    private readonly ICustomerFacade _customerFacade;
    public AuthenticateController(UserManager<ApplicationUser> userManager, IOptions<OtpConfig> config, IOptions<JwtBearerTokenSettings> jwt, IOptions<TokenExpirationConfig> tokenExpiration, 
        RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenGenerator tokenGenerator, IOtpSmsService otpSmsService, ICustomerFacade customerFacade)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
        _otpSmsService = otpSmsService;
        _otpConfig = config.Value;
        _tokenExpirationConfig = tokenExpiration.Value;
        _jwt = jwt.Value;
        _customerFacade= customerFacade;
    }

    [HttpGet("{username}/approaches")]
    public async Task<IActionResult> Approaches(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BadHttpRequestException("invalid username");

        await _otpSmsService.SendOtpCodeAsync(username, GenerateOtpCode(_otpConfig.CodeLength));
        var otpResponse = new OtpResponse(_tokenExpirationConfig.OtpExpirationTime);
        var result = new Response<OtpResponse>(otpResponse);

        return Ok(result);
    }


    [HttpPost("login")]
    public async Task<ActionResult<Response<AuthenticationResponse>>> LoginWithOtp([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            var otpResult = await _otpSmsService.ValidationOtp(model.Username, model.Otp);
            if (!otpResult)
                throw new UnauthorizedException("Unauthorized");

            var applicationUser = new ApplicationUser()
            {
                Email = $"{Guid.NewGuid()}@ofood.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(applicationUser);
            if (!result.Succeeded)
                throw new BadHttpRequestException("User creation failed! Please check user details and try again.");

            if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

            if (await _roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await _userManager.AddToRoleAsync(applicationUser, UserRoles.Customer);
            }

            await _customerFacade.CreateCustomerAsync(new CreateCustomerRequest(Guid.Parse(applicationUser.Id), model.Username));

            var tokenKey = _tokenGenerator.GenerateToken(new TokenGenerationSettings()
            {
                Role = Role.Customeer,
                SecurityKey = _jwt.SecretKey,
                UserId = applicationUser.Id,
                Username = applicationUser.UserName!
            });


            var dto = AuthenticationResponse.Build(tokenKey, DateTime.UtcNow.AddDays(30));
            var regUser = new Response<AuthenticationResponse>(dto);

            return Ok(regUser);
        }
        else
        {
            var otpResult = await _otpSmsService.ValidationOtp(model.Username, model.Otp);

            if (!otpResult)
                throw new UnauthorizedException("Unauthorized");

            var tokenKey = _tokenGenerator.GenerateToken(new TokenGenerationSettings()
            {
                Role = Role.Customeer,
                SecurityKey = _jwt.SecretKey,
                UserId = user.Id,
                Username = user.UserName!
            });
            var dto = AuthenticationResponse.Build(tokenKey, DateTime.UtcNow.AddDays(30));
            var regUser = new Response<AuthenticationResponse>(dto);
            return Ok(regUser);
        }
    }

#if DEBUG
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        ApplicationUser user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new BadHttpRequestException("User creation failed! Please check user details and try again.");


        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        return Ok();
    }
#endif


    private string GenerateOtpCode(int length)
    {
        const string chars = "1234567890";
        var stringChars = new char[length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}
