using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCCCTH2
{
    [Serializable]
    public class FXBCCCTH2Model
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Totals footer (used by report Designer)
        public decimal T_ngia1 { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public decimal T_gt_cl1 { get; set; }
        public List<FXBCCCTH2Row> LsData { get; set; }

        public FXBCCCTH2Model()
        {
            LsData = new List<FXBCCCTH2Row>();
        }
    }

    public class FXBCCCTH2Row
    {
        public int Stt { get; set; }

        // Group-by criteria fields
        public string Loai_cc { get; set; }
        public string Ten_loai { get; set; }
        public string Ten_loai2 { get; set; }

        public string Ma_bpcc { get; set; }
        public string Ten_bpts { get; set; }
        public string Ten_bpts2 { get; set; }

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
        public string Ten_nh1_2 { get; set; }

        public string Nh_cc2 { get; set; }
        public string Ten_nh2 { get; set; }
        public string Ten_nh2_2 { get; set; }

        public string Nh_cc3 { get; set; }
        public string Ten_nh3 { get; set; }
        public string Ten_nh3_2 { get; set; }

        // Data fields
        public decimal Ngia1 { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl1 { get; set; }

        public int Bold { get; set; }
    }
}
