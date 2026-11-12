using System;
using System.Data;
using System.Threading.Tasks;
using SisData.Data;

namespace SisCom.Services.Totp
{
    public class TotpSettingService : ITotpSettingService
    {
        private readonly IDbManager _db;

        public TotpSettingService(IDbManager db)
        {
            _db = db;
        }

        public Task<bool> GetEnabledAsync()
        {
            try
            {
                DataSet ds = _db.LoadDataSet("SELECT TOP 1 is_enabled FROM sys_totp_setting");
                bool enabled = ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                    && Convert.ToBoolean(ds.Tables[0].Rows[0]["is_enabled"]);
                return Task.FromResult(enabled);
            }
            catch
            {
                // Bang sys_totp_setting chua duoc tao (chua chay Doc/Totp_Setup.sql)
                // => coi nhu tinh nang TOTP tat, cho phep dang nhap binh thuong.
                return Task.FromResult(false);
            }
        }

        public async Task SetEnabledAsync(bool enabled, string updatedBy)
        {
            // Tao bang + dong cau hinh neu chua co (idempotent), tuong duong Doc/Totp_Setup.sql.
            // Nho vay admin bat TOTP lan dau khong bi loi "Invalid object name".
            await EnsureSchemaAsync();

            string safeUpdatedBy = (updatedBy ?? "").Replace("'", "''");
            string sql = string.Format(
                "UPDATE sys_totp_setting SET is_enabled = {0}, updated_by = N'{1}', updated_date = GETDATE()",
                enabled ? 1 : 0, safeUpdatedBy);
            await _db.ExecuteSqlQueryAsync(sql);
        }

        private async Task EnsureSchemaAsync()
        {
            string sql =
                "IF OBJECT_ID('sys_totp_setting','U') IS NULL " +
                "CREATE TABLE sys_totp_setting (" +
                "  id INT IDENTITY PRIMARY KEY," +
                "  is_enabled BIT NOT NULL DEFAULT 0," +
                "  updated_by VARCHAR(50) NULL," +
                "  updated_date DATETIME NULL" +
                "); " +
                "IF NOT EXISTS (SELECT 1 FROM sys_totp_setting) " +
                "INSERT INTO sys_totp_setting(is_enabled) VALUES (0); " +
                "IF OBJECT_ID('user_totp','U') IS NULL " +
                "CREATE TABLE user_totp (" +
                "  user_id INT NOT NULL PRIMARY KEY," +
                "  secret_key VARCHAR(100) NOT NULL," +
                "  is_confirmed BIT NOT NULL DEFAULT 0," +
                "  created_date DATETIME NOT NULL DEFAULT GETDATE()," +
                "  confirmed_date DATETIME NULL" +
                ");";
            await _db.ExecuteSqlQueryAsync(sql);
        }
    }
}
