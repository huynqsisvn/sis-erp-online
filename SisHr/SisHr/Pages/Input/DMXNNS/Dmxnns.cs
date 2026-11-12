using System;

namespace SisHr.Pages.Input.DMXNNS
{
    public class DmxnnsRow
    {
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Dien_thoai { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ma_ns_nhan { get; set; }
        public string Ma_noinhan { get; set; }
        public string Dia_chi { get; set; } = ""; // Trả lại tên cũ là Dia_chi
        public string Dia_chi_giao { get; set; }
        public string Noi_dung { get; set; }
        public string Ghi_chu { get; set; }
        public decimal So_luong { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display fields
        public string Ten_ns_giao { get; set; }
        public string Ma_bpns_giao { get; set; }
        public string Ten_ns_nhan_display { get; set; }
        public string Ma_bpns_nhan { get; set; }
    }

    public class DmxnnsDtRow
    {
        public string Ma_ns { get; set; }
        public string Ten_giayto { get; set; }
        public bool Ban_chinh { get; set; }
        public bool Ban_sao { get; set; }
        public int So_luong_banchinh { get; set; }
        public int So_luong_bansao { get; set; }
        public decimal So_luong { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ghi_chu { get; set; }
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
