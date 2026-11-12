using System;

namespace SisHr.Pages.Input.HRCPCCDC
{
    public class HrcpccdcRow
    {
        public string So_ct { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string Dien_giai { get; set; } = "";
        public string Ma_qs { get; set; } = "";
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

    public class HrcpccdcDtRow
    {
        public string So_ct { get; set; } = "";
        public string Ma_ns { get; set; } = "";
        public string Ten_ns { get; set; } = "";
        public string Ma_bpns { get; set; } = "";
        public string Ten_bpns { get; set; } = "";
        public string Ma_chucvu { get; set; } = "";
        public string Ma_cc_me { get; set; } = "";
        public string Ten_cc { get; set; } = "";
        public decimal So_luong { get; set; }
        public DateTime? Ngay_cap { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ghi_chu { get; set; } = "";
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
