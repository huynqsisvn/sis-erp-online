using System;

namespace SisHr.Pages.Input.HRNVN
{
    public class Hrnvn
    {
        public string Ma_ns { get; set; }
        public DateTime? Ngay_lam_don { get; set; }
        public DateTime? Ngay_thoi_viec { get; set; }
        public DateTime? Ngay_thoi_thuc_te { get; set; }
        public decimal? Tro_cap { get; set; }
        public DateTime? Ngay_chot_so { get; set; }
        public DateTime? Ngay_tra_so { get; set; }
        public string Quyet_dinh_cham_dut_hd { get; set; }
        public string Quyet_dinh_tro_cap { get; set; }
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
    }
}
