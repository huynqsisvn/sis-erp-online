using System;

namespace SisHr.Pages.Input.HRDMKT
{
    public class HrdmktRow
    {
        public string Ktid { get; set; }  // IDENTITY column - let DB generate, can be null before insert
        public string Ma_ns { get; set; }
        public string Ma_quyet_dinh { get; set; }
        public DateTime? Ngay_cap_nhat { get; set; }
        public string Ma_hinh_thuc { get; set; }
        public int? So_lan_thuong_phat { get; set; }
        public string Ly_do_thuong { get; set; }
        public string Ly_do_phat { get; set; }
        public string Ghi_chu { get; set; }
        public string Status { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display fields (from view)
        public string Ten_ns { get; set; }
        public string Ten_hinh_thuc { get; set; }
    }
}
