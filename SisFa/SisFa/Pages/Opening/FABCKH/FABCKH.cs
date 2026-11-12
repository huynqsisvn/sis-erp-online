using System;
using System.Collections.Generic;

namespace SisFa.Pages.Opening.FABCKH
{
    [Serializable]
    public class FABCKHModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        // Khai báo thêm các biến tổng trực tiếp cho báo cáo
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_kh_trong_ky { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }

        public List<FABCKHRow> LsData { get; set; }

        // Khai báo thêm list Tong đề phòng mẫu in dùng Table tổng
        public List<lTC> Tong { get; set; }

        public FABCKHModel()
        {
            LsData = new List<FABCKHRow>();
            Tong = new List<lTC>();
        }
    }

    public class FABCKHRow
    {
        public int Stt { get; set; }
        public string Loai_ts { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_bpsd { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal So_ky_kh { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_kh_trong_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public string Nh_ts { get; set; }
        public string Tk_ts { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }

    // Class hỗ trợ Binding dữ liệu dòng tổng
    public class lTC
    {
        public decimal nguyen_gia { get; set; }
        public decimal gt_kh_ky { get; set; }
        public decimal gt_kh_trong_ky { get; set; }
        public decimal gt_da_kh { get; set; }
        public decimal gt_cl { get; set; }
    }
}