using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCNV
{
    [Serializable]
    public class FXBCNVModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public string Title { get; set; }
        // Tổng footer
        public decimal T_sl_cc { get; set; }
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FXBCNVRow> LsData { get; set; }

        public FXBCNVModel()
        {
            LsData = new List<FXBCNVRow>();
        }
    }

    public class FXBCNVRow
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
    }
}
