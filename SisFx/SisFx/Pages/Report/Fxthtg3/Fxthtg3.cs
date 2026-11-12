using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.Fxthtg3
{
    [Serializable]
    public class Fxthtg3Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<Fxthtg3Row> LsData { get; set; }

        public Fxthtg3Model()
        {
            LsData = new List<Fxthtg3Row>();
        }
    }

    public class Fxthtg3Row
    {
        public int Stt { get; set; }

        // Group-by criteria fields
        public string Ma_tg_cc { get; set; }
        public string Ten_tg_cc { get; set; }
        public string Ten_tg_cc2 { get; set; }

        public string Loai_cc { get; set; }
        public string Ten_loai { get; set; }
        public string Ten_loai2 { get; set; }

        public string Ma_bpcc { get; set; }
        public string Ten_bpcc { get; set; }
        public string Ten_bpcc2 { get; set; }

        public string Nh_cc { get; set; }
        public string Ten_nh_cc { get; set; }
        public string Ten_nh_cc2 { get; set; }

        public string Tk_cp { get; set; }
        public string Ten_tk_cp { get; set; }
        public string Ten_tk_cp2 { get; set; }

        public string Ma_dvcs { get; set; }
        public string Ten_dvcs { get; set; }
        public string Ten_dvcs2 { get; set; }

        public string Nh_cc1 { get; set; }
        public string Ten_nh1 { get; set; }
        public string Ten_nh21 { get; set; }

        public string Nh_cc2 { get; set; }
        public string Ten_nh2 { get; set; }
        public string Ten_nh22 { get; set; }

        public string Nh_cc3 { get; set; }
        public string Ten_nh3 { get; set; }
        public string Ten_nh23 { get; set; }

        // Data fields
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
    }
}
