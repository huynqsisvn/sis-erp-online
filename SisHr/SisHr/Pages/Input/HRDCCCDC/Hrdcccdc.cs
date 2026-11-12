using System;

namespace SisHr.Pages.Input.HRDCCCDC
{
    public class HrdccccdcRow
    {
        public string So_ct { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string Dien_giai { get; set; } = "";
        public string Ma_qs { get; set; } = "";
        public string Loai { get; set; } = "0";
        public string Ten_loai { get; set; } = "";
        public int? Stt_rec { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
    }

    public class HrdccccdcDtRow
    {
        public string So_ct { get; set; } = "";
        public string Ma_ns_tu { get; set; } = "";
        public string Ten_ns_tu { get; set; } = "";
        public string Ma_ns_sang { get; set; } = "";
        public string Ten_ns_sang { get; set; } = "";
        public string Ma_cc_me { get; set; } = "";
        public string Ten_cc { get; set; } = "";
        public decimal So_luong { get; set; }
        public DateTime? Ngay_cap { get; set; }
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_cap { get; set; } = "";
        public string Ly_do_dc { get; set; } = "";
        public int? Stt_rec { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // UI helper
        public int Stt { get; set; }
    }
}
