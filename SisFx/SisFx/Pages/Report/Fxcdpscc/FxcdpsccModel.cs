using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.Fxcdpscc
{
    [Serializable]
    public class FxcdpsccModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        // Số đầu kỳ
        public decimal T_nguyen_gia_dk { get; set; }
        public decimal T_gt_kh_dk { get; set; }
        public decimal T_gt_cl_dk { get; set; }

        // Số phát sinh
        public decimal T_nguyen_gia_tg { get; set; }
        public decimal T_gt_kh_tg { get; set; }
        public decimal T_gt_kh_ky { get; set; }

        // Số cuối kỳ
        public decimal T_nguyen_gia_ck { get; set; }
        public decimal T_gt_kh_ck { get; set; }
        public decimal T_gt_cl_ck { get; set; }

        public List<FxcdpsccRow> LsData { get; set; }
        public List<FxcdpsccTong> Tong { get; set; }

        public FxcdpsccModel()
        {
            LsData = new List<FxcdpsccRow>();
            Tong = new List<FxcdpsccTong>();
        }
    }

    public class FxcdpsccRow
    {
        public int Stt { get; set; }
        public int Ftag { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Loai_cc { get; set; }
        public string Nh_cc { get; set; }
        public string Ten_nh_cc { get; set; }
        public string Ten_nh_cc2 { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_dvcs { get; set; }
        public decimal Sl_cc { get; set; }
        public int So_ky_kh { get; set; }
        public DateTime? Ngay_tang { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public DateTime? Ngay_giam { get; set; }

        // Số đầu kỳ
        public decimal nguyen_gia_dk { get; set; }
        public decimal gt_kh_dk { get; set; }
        public decimal gt_cl_dk { get; set; }

        // Số phát sinh tăng/giảm nguyên giá & KH
        public decimal nguyen_gia_tang { get; set; }
        public decimal nguyen_gia_giam { get; set; }
        public decimal gt_kh_tang { get; set; }
        public decimal gt_kh_giam { get; set; }

        // Phân bổ trong kỳ
        public decimal gt_kh_ky { get; set; }

        // Số cuối kỳ
        public decimal nguyen_gia_ck { get; set; }
        public decimal gt_kh_ck { get; set; }
        public decimal gt_cl_ck { get; set; }

        // Tổng tăng/giảm (= tăng - giảm)
        public decimal nguyen_gia_tg { get; set; }
        public decimal gt_kh_tg { get; set; }
    }

    public class FxcdpsccTong
    {
        public decimal nguyen_gia_dk { get; set; }
        public decimal gt_kh_dk { get; set; }
        public decimal gt_cl_dk { get; set; }

        public decimal nguyen_gia_tg { get; set; }
        public decimal gt_kh_tg { get; set; }
        public decimal gt_kh_ky { get; set; }

        public decimal nguyen_gia_ck { get; set; }
        public decimal gt_kh_ck { get; set; }
        public decimal gt_cl_ck { get; set; }
    }
}
