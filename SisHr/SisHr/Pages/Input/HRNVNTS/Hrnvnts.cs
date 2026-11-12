using System;

namespace SisHr.Pages.Input.HRNVNTS
{
    public class Hrnvnts
    {
        public string Idts { get; set; }
        public string Ma_ns { get; set; }
        public string Ma_ts { get; set; }
        public string Ten_ts { get; set; }
        public DateTime? Ngay_lam_don { get; set; }
        public DateTime? Ngay_nghi { get; set; }
        public DateTime? Ngay_di_lam_lai { get; set; }
        public decimal? Tro_cap { get; set; }
        public string Quyet_dinh_tro_cap { get; set; }
        public string Ly_do { get; set; }
        public string Ghi_chu { get; set; }
        public int? Status { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display fields
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        public string Ma_bpns { get; set; }
        public string Ten_bpns { get; set; }
    }
}
