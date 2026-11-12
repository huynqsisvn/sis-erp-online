using System;

namespace SisHr.Pages.Input.DMDXCP
{
    public class DmdxcpRow
    {
        public string So_ct { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string Ma_ns { get; set; } = "";
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display fields (not mapped to DB columns)
        public string Ten_ns { get; set; } = "";
        public string Ten_bpns { get; set; } = "";
    }

    public class DmdxcpDtRow
    {
        public string So_ct { get; set; } = "";
        public string Noi_dung { get; set; } = "";
        public string Dvt { get; set; } = "";
        public decimal So_luong { get; set; }
        public decimal Don_gia { get; set; }
        public decimal Thanh_tien { get; set; } // Tính toán, không l?u DB
        public string Ghi_chu { get; set; } = "";
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
