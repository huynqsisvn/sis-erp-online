using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Model
{
    public class Notification2cus
    {
        public Guid NotificationID { get; set; }
        public DateTime Ngay1 { get; set; }
        public DateTime Ngay2 { get; set; }
        public string Ma_kh { get; set; }
        public string Ma_sp { get; set; }
        public int stt1 { get; set; }
        public string No_type { get; set; }
        public string Tieu_de { get; set; }
        public string Noi_dung { get; set; }
        public string Ghi_chu { get; set; }
        public string Img_link { get; set; }
        public string No_link { get; set; }
        public string Users_check { get; set; }
        public string Users_notshow { get; set; }
        public string Users_show { get; set; }
        public string Status { get; set; }
        public int User_id2 { get; set; }
        public int User_id0 { get; set; }
        public string Time2 { get; set; }
        public string Time0 { get; set; }
        public DateTime Date2 { get; set; }
        public DateTime Date0 { get; set; }
    }
}
