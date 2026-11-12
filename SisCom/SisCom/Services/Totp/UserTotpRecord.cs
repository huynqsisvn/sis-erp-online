using System;

namespace SisCom.Services.Totp
{
    public class UserTotpRecord
    {
        public int UserId { get; set; }
        public string SecretKey { get; set; } = "";
        public bool IsConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
    }
}
