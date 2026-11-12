using System;
using System.Data;
using System.Threading.Tasks;
using OtpNet;
using QRCoder;
using SisData.Data;

namespace SisCom.Services.Totp
{
    public class UserTotpService : IUserTotpService
    {
        private readonly IDbManager _db;

        public UserTotpService(IDbManager db)
        {
            _db = db;
        }

        public Task<UserTotpRecord?> GetByUserIdAsync(int userId)
        {
            DataSet ds = _db.LoadDataSet(string.Format(
                "SELECT user_id, secret_key, is_confirmed, created_date, confirmed_date FROM user_totp WHERE user_id = {0}",
                userId));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return Task.FromResult<UserTotpRecord?>(null);

            DataRow row = ds.Tables[0].Rows[0];
            var record = new UserTotpRecord
            {
                UserId = Convert.ToInt32(row["user_id"]),
                SecretKey = row["secret_key"].ToString() ?? "",
                IsConfirmed = Convert.ToBoolean(row["is_confirmed"]),
                CreatedDate = Convert.ToDateTime(row["created_date"]),
                ConfirmedDate = row["confirmed_date"] == DBNull.Value ? null : Convert.ToDateTime(row["confirmed_date"])
            };
            return Task.FromResult<UserTotpRecord?>(record);
        }

        public async Task<string> EnsureSecretAsync(int userId)
        {
            var existing = await GetByUserIdAsync(userId);
            if (existing != null)
                return existing.SecretKey;

            string secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
            string safeSecret = secret.Replace("'", "''");
            string sql = string.Format(
                "INSERT INTO user_totp(user_id, secret_key, is_confirmed, created_date) VALUES ({0}, '{1}', 0, GETDATE())",
                userId, safeSecret);
            await _db.ExecuteSqlQueryAsync(sql);
            return secret;
        }

        public async Task ConfirmAsync(int userId)
        {
            string sql = string.Format(
                "UPDATE user_totp SET is_confirmed = 1, confirmed_date = GETDATE() WHERE user_id = {0}",
                userId);
            await _db.ExecuteSqlQueryAsync(sql);
        }

        // Xoa dang ky TOTP cua user de lan dang nhap sau tao QR/secret moi (dung khi app Authenticator giu secret cu, lech voi DB).
        public async Task ResetAsync(int userId)
        {
            string sql = string.Format("DELETE FROM user_totp WHERE user_id = {0}", userId);
            await _db.ExecuteSqlQueryAsync(sql);
        }

        // Tra ve false neu khong tim thay user; true neu da xoa (hoac user khong co dang ky nao).
        public async Task<bool> ResetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            string safeName = username.Trim().Replace("'", "''");
            DataSet ds = _db.LoadDataSet(string.Format(
                "SELECT user_id FROM userinfo_web WHERE user_name = '{0}'", safeName));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;

            int userId = Convert.ToInt32(ds.Tables[0].Rows[0]["user_id"]);
            await ResetAsync(userId);
            return true;
        }

        public async Task<bool> VerifyCodeAsync(int userId, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            var record = await GetByUserIdAsync(userId);
            if (record == null)
                return false;

            var totp = new OtpNet.Totp(Base32Encoding.ToBytes(record.SecretKey));
            string entered = code.Trim();
            // TAM THOI: dong ho server dang lech ~3 phut so voi dien thoai. Noi cua so cham nhan len +-10 buoc (~+-5 phut)
            // de dang nhap duoc trong khi cho dong bo lai gio server (NTP). Sau khi chinh gio xong nen tra ve VerificationWindow(2, 2).
            bool ok = totp.VerifyTotp(entered, out long window, new VerificationWindow(10, 10));
            Log($"userId={userId}, secretLen={record.SecretKey?.Length}, serverUtcNow={DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}, " +
                $"expectedCode={totp.ComputeTotp()}, enteredCode={entered}, matchedStep={window}, result={ok}");

            // --- CHAN DOAN: quet +-20 buoc (~+-10 phut) de phan biet LECH DONG HO vs SAI SECRET ---
            if (!ok)
            {
                DateTime nowUtc = DateTime.UtcNow;
                int? matchOffset = null;
                for (int step = -20; step <= 20; step++)
                {
                    string c = totp.ComputeTotp(nowUtc.AddSeconds(step * 30));
                    if (c == entered) { matchOffset = step; break; }
                }
                if (matchOffset.HasValue)
                    Log($"[DIAG] userId={userId}: ma khop tai offset {matchOffset} buoc (~{matchOffset * 30}s). => LECH DONG HO, can chinh lai gio server/dien thoai.");
                else
                    Log($"[DIAG] userId={userId}: khong khop o bat ky offset +-10 phut nao. => SAI SECRET (app giu secret khac DB).");
            }
            return ok;
        }

        // Ghi log chan doan ra file de doi chieu expected vs entered (xac dinh loi lech dong ho hay sai secret).
        private static void Log(string message)
        {
            try
            {
                System.IO.File.AppendAllText(@"..\Totp_debug.txt",
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [TOTP] {message}{Environment.NewLine}");
            }
            catch { }
        }

        public string BuildOtpAuthUri(string secretBase32, string username, string issuer = "SIS ERP")
        {
            string encodedIssuer = Uri.EscapeDataString(issuer);
            string encodedUser = Uri.EscapeDataString(username);
            // Khai bao ro period/algorithm de Microsoft Authenticator khong doan sai tham so.
            return string.Format(
                "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6&period=30&algorithm=SHA1",
                encodedIssuer, encodedUser, secretBase32);
        }

        public string BuildQrCodePngBase64(string otpAuthUri)
        {
            using var generator = new QRCodeGenerator();
            using var qrCodeData = generator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q);
            var pngQrCode = new PngByteQRCode(qrCodeData);
            byte[] bytes = pngQrCode.GetGraphic(20);
            return Convert.ToBase64String(bytes);
        }
    }
}
