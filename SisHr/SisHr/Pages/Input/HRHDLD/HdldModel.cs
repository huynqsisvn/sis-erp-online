using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRHDLD
{
    [Serializable]
    public class HdldModel
    {
        public List<HdldDetail> LsData { get; set; } = new List<HdldDetail>();

        public HdldModel()
        {
            LsData = new List<HdldDetail>();
        }
    }

    [Serializable]
    public class HdldDetail
    {
        public string Ma_hdld { get; set; }
        public string Ma_plhd { get; set; }
        public string Ma_ns_ct { get; set; }
        public string Ma_ns { get; set; }
        public string Ma_lhd { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
        public string Dia_diem_lam_viec { get; set; }
        public string Dia_chi_lam_viec { get; set; }
        public string Ma_cv_ct { get; set; }
        public string Ma_cv { get; set; }
        public string Yeu_cau1 { get; set; }
        public string Yeu_cau2 { get; set; }
        public int? Thoi_gian_lv { get; set; }
        public string Duoc_cap_nhung_phuong_tien { get; set; }
        public string Phuong_tien { get; set; }
        public int? Muc_luong { get; set; }
        public string Phu_cap { get; set; }
        public DateTime? Ngay_hl { get; set; }
        public DateTime? Ngay_hd { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
        public int Status { get; set; }

        // Display fields - Thông tin công ty (tbCTYInfo)
        public string Ten_ns_ct { get; set; }
        public string Ten_cv_ct { get; set; }
        public string Ten_cv_ct_2 { get; set; }
        public string Tencty { get; set; }
        public string Dien_thoai_ct { get; set; }
        public string Diachicty { get; set; }
        public string Quoc_tich_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }

        // Display fields - Thông tin nhân viên
        public string Ten_ns { get; set; }
        public string Quoc_tich { get; set; }
        public string Ten_ns2 { get; set; }
        public string Quoc_tich2 { get; set; }
        public string Ngay_sinh { get; set; }
        public string Dia_chi { get; set; }
        public string Cmnd { get; set; }
        public string Ngay_cmnd { get; set; }
        public string Nc_cmnd { get; set; }

        // Display fields - Thông tin h?p ??ng
        public string Ma_quocgia { get; set; }
        public string Ten_lhdong { get; set; }
        public string Ten_cv { get; set; }
    }
}
