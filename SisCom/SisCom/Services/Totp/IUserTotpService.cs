using System.Threading.Tasks;

namespace SisCom.Services.Totp
{
    public interface IUserTotpService
    {
        Task<UserTotpRecord?> GetByUserIdAsync(int userId);
        Task<string> EnsureSecretAsync(int userId);
        Task ConfirmAsync(int userId);
        Task<bool> VerifyCodeAsync(int userId, string code);
        Task ResetAsync(int userId);
        Task<bool> ResetByUsernameAsync(string username);
        string BuildOtpAuthUri(string secretBase32, string username, string issuer = "SIS ERP");
        string BuildQrCodePngBase64(string otpAuthUri);
    }
}
