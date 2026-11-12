using System;

namespace SisHr.Pages.Input.HRPLHD
{
    public class Plhd
    {
        public string Ma_hdld { get; set; }
        public string Ma_plhd2 { get; set; }
        public string Noi_dung_thay_doi { get; set; }
        public int? Luong_co_ban { get; set; }
        public string Thoi_gian_thuc_hien { get; set; }
        public DateTime? Ngay_ap_dung { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
        public int Status { get; set; }

        // Display fields
        public string Ten_hdld { get; set; }
        public string Tencty { get; set; }
        public string Diachicty { get; set; }
        public string Ten_ns_ct { get; set; }
        public string Ten_ns_ct_2 { get; set; }
        public string Ten_cv_ct { get; set; }
        public string Ten_cv_ct_2 { get; set; }
        public string Dien_thoai_ct { get; set; }
        public string Quoc_tich_ct { get; set; }
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        public string Quoc_tich { get; set; }
        public string Quoc_tich2 { get; set; }
        public string Ngay_sinh { get; set; }
        public string Dia_chi { get; set; }
        public string Cmnd { get; set; }
        public string Ngay_cmnd { get; set; }
        public string Nc_cmnd { get; set; }
    }
}
