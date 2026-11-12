using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.Fxbkcccbp
{
    [Serializable]
    public class FXBKCCCBPModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Chitietsl { get; set; }
        public List<FXBKCCCBPRow> LsData { get; set; }

        public FXBKCCCBPModel()
        {
            LsData = new List<FXBKCCCBPRow>();
        }
    }

    public class FXBKCCCBPRow
    {
        public int Stt { get; set; }
        public int Ftag { get; set; }
        public int Chuyen_bp { get; set; }
        public string Ma_loai { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_bpcc0 { get; set; }
        public string Ma_bpcc { get; set; }
        public int Ky { get; set; }
        public int Nam { get; set; }
        public decimal Sl_cc { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public string Ghi_chu { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public decimal Nguyen_gia { get; set; }
    }
}
