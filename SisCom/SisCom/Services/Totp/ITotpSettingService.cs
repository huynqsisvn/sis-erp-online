using System.Threading.Tasks;

namespace SisCom.Services.Totp
{
    public interface ITotpSettingService
    {
        Task<bool> GetEnabledAsync();
        Task SetEnabledAsync(bool enabled, string updatedBy);
    }
}
