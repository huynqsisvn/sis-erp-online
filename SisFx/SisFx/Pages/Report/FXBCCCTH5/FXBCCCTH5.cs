using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCCCTH5
{
    [Serializable]
    public class FXBCCCTH5Model
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng cộng footer (dùng bởi report Designer)
        public decimal T_sl_cc { get; set; }
        public decimal T_ngia1 { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public decimal T_gt_cl1 { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh0 { get; set; }
        public List<FXBCCCTH5Row> LsData { get; set; }

        public FXBCCCTH5Model()
        {
            LsData = new List<FXBCCCTH5Row>();
        }
    }

    public class FXBCCCTH5Row
    {
        public int Stt { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_tg_cc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public DateTime Ngay_mua { get; set; }
        public string Ten_kieu_pb { get; set; }
        public string Ten_kieu_pb2 { get; set; }
        public int So_ky_kh { get; set; }
        public DateTime Ngay_kh0 { get; set; }
        public DateTime Ngay_kt_kh { get; set; }
        public DateTime Ngay_giam { get; set; }
        public DateTime Ngay_kh1 { get; set; }
        public string Ly_do_giam { get; set; }
        public string Tk_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string Nh_cc { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Nuoc_sx { get; set; }
        public int Nam_sx { get; set; }
        public string So_hieu_cc { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_giam_cc { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Ngia1 { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh0 { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ten_giam { get; set; }
        public string Ten_giam2 { get; set; }
        public string Ten_tang { get; set; }
        public string Ten_tang2 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
