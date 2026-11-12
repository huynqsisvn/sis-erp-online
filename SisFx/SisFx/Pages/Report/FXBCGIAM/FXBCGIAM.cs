using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCGIAM
{
    [Serializable]
    public class FXBCGIAMModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FXBCGIAMRow> LsData { get; set; }

        public FXBCGIAMModel()
        {
            LsData = new List<FXBCGIAMRow>();
        }
    }

    public class FXBCGIAMRow
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public string Ma_cc { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_bpcc2 { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_cc_me { get; set; }
        public DateTime? Ngay_tg { get; set; }
        public string Ten_tg_cc { get; set; }
        public string Ten_tg_cc2 { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string So_ct_trim { get; set; }
        public string Ghi_chu { get; set; }
        public int Tag { get; set; }
    }
}
