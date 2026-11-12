using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCPBKH
{
    [Serializable]
    public class FXBCPBKHModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }

        public List<FXBCPBKHRow> LsData { get; set; }

        public FXBCPBKHModel()
        {
            LsData = new List<FXBCPBKHRow>();
        }
    }

    public class FXBCPBKHRow
    {
        public int Stt { get; set; }
        public int Nam { get; set; }
        public int Ky { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_bpcc { get; set; }
        public string Tk_pb { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string So_lsx { get; set; }
        public string Ma_sp { get; set; }
        public string Ma_dvcs { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Nguyen_gia1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public string Nh_cc { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
