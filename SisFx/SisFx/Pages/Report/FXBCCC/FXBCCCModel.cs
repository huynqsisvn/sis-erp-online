using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCCC
{
    [Serializable]
    public class FXBCCCModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng footer
        public decimal T_sl_cc { get; set; }
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FXBCCCRow> LsData { get; set; }

        public FXBCCCModel()
        {
            LsData = new List<FXBCCCRow>();
        }
    }

    public class FXBCCCRow
    {
        public int Stt { get; set; }
        public int Ftag { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_cc { get; set; }
        public DateTime Ngay_mua { get; set; }
        public int So_ky_kh { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_bpcc { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ten_bpts { get; set; }
        public string Ten_bpts2 { get; set; }
        public string Ten_loai { get; set; }
        public string Ten_loai2 { get; set; }
        public int Bold { get; set; }
    }
}
