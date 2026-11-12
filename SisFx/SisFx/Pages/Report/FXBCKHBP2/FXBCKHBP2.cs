using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCKHBP2
{
    [Serializable]
    public class FXBCKHBPModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_bpsd { get; set; }
        public string LoaiBC { get; set; }

        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }

        public List<FXBCKHBPRow> LsData { get; set; }
        public List<FXBCKHBPTong> Tong { get; set; }

        public FXBCKHBPModel()
        {
            LsData = new List<FXBCKHBPRow>();
            Tong = new List<FXBCKHBPTong>();
        }
    }

    public class FXBCKHBPRow
    {
        public int Stt { get; set; }
        public string Ma_bpsd { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string So_the_ts { get; set; }
        public string Ma_cc { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal So_ky_kh { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Gt_kh_1_ky { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Nguyen_gia1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_nv { get; set; }
        public string Ma_dvcs { get; set; }
        public int Ftag { get; set; }
        public int Bold { get; set; }
    }

    public class FXBCKHBPTong
    {
        public decimal nguyen_gia { get; set; }
        public decimal gt_kh_ky { get; set; }
        public decimal gt_da_kh { get; set; }
        public decimal gt_cl { get; set; }
    }
}
