using System;
using System.Collections.Generic;

namespace SisFa.Pages.Opening.Facdpsts
{
    [Serializable]
    public class FacdpstsModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        // BỔ SUNG 9 BIẾN TỔNG CHO MẪU IN 
        // 1. Số đầu kỳ
        public decimal T_nguyen_gia_dk { get; set; }
        public decimal T_gt_kh_dk { get; set; }
        public decimal T_gt_cl_dk { get; set; }

        // 2. Số phát sinh
        public decimal T_nguyen_gia_tg { get; set; }
        public decimal T_gt_kh_tg { get; set; }
        public decimal T_gt_kh_ky { get; set; }

        // 3. Số cuối kỳ
        public decimal T_nguyen_gia_ck { get; set; }
        public decimal T_gt_kh_ck { get; set; }
        public decimal T_gt_cl_ck { get; set; }

        public List<FacdpstsRow> LsData { get; set; }
        public List<lTC> Tong { get; set; }

        public FacdpstsModel()
        {
            LsData = new List<FacdpstsRow>();
            Tong = new List<lTC>();
        }
    }

    public class FacdpstsRow
    {
        public int Stt { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Tk_cp { get; set; }
        public DateTime? Ngay_tang { get; set; }
        public decimal So_ky_kh { get; set; }

        public decimal nguyen_gia_dk { get; set; }
        public decimal gt_kh_dk { get; set; }
        public decimal gt_cl_dk { get; set; }

        public decimal nguyen_gia_tg { get; set; }
        public decimal gt_kh_tg { get; set; }
        public decimal gt_kh_ky { get; set; }

        public decimal nguyen_gia_ck { get; set; }
        public decimal gt_kh_ck { get; set; }
        public decimal gt_cl_ck { get; set; }

        public string ma_bpts { get; set; }
        public string loai_ts { get; set; }
        public string nh_ts { get; set; }
        public string tk_kh { get; set; }
        public string nh_ts1 { get; set; }
        public string nh_ts2 { get; set; }
        public string nh_ts3 { get; set; }
        public DateTime? ngay_kh0 { get; set; }
        public DateTime? ngay_kh1 { get; set; }
        public DateTime? ngay_giam { get; set; }
        public string ma_nv { get; set; }

        // Alternate names used by some report templates
        public decimal nguyen_gia1 { get; set; }
        public decimal gt_da_kh1 { get; set; }
        public decimal gt_cl1 { get; set; }

        public int Bold { get; set; }
        public string Ftag { get; set; }
    }

    public class lTC
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
