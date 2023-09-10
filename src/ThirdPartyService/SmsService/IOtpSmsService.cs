namespace SmsService
{
    public interface IOtpSmsService
    {
        Task<int> SendOtpCodeAsync(string username, string otpCode);

        Task<bool> ValidationOtp(string username, string otpCode);
    }
}