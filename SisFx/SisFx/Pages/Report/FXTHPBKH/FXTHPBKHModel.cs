using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXTHPBKH
{
    [Serializable]
    public class FXTHPBKHModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public decimal T_gt_pb { get; set; }

        public List<FXTHPBKHRow> LsData { get; set; }

        public FXTHPBKHModel()
        {
            LsData = new List<FXTHPBKHRow>();
        }
    }

    public class FXTHPBKHRow
    {
        public long Stt { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_vv { get; set; }
        public string So_lsx { get; set; }
        public string Ma_sp { get; set; }
        public decimal Gt_pb { get; set; }
        public string Ten_bpht { get; set; }
        public string Ten_bpht2 { get; set; }
        public string Ten_phi { get; set; }
        public string Ten_phi2 { get; set; }
        public string Ten_vv { get; set; }
        public string Ten_vv2 { get; set; }
        public DateTime? Ngay_lkh { get; set; }
        public string Ten_sp { get; set; }
        public string Ten_sp2 { get; set; }
    }
}
