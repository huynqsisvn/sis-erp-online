using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCCCTH1
{
    [Serializable]
    public class FXBCCCTH1Model
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng footer
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FXBCCCTH1Row> LsData { get; set; }

        public FXBCCCTH1Model()
        {
            LsData = new List<FXBCCCTH1Row>();
        }
    }

    public class FXBCCCTH1Row
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public string Nhom { get; set; }
        public decimal Ngia1 { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl1 { get; set; }
        public int Ftag { get; set; }
    }
}
