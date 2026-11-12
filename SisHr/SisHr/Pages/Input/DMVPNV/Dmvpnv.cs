using System;

namespace SisHr.Pages.Input.DMVPNV
{
    public class DmvpnvRow
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
    }

    public class DmvpnvDtRow
    {
        public string So_ct { get; set; } = "";
        public string Noi_dung_ky_luat { get; set; } = "";
        public string Hinh_thuc_ky_luat { get; set; } = "";
        public decimal Muc_do_vi_pham { get; set; }
        public string Trang_thai_xu_ly { get; set; } = "";
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
