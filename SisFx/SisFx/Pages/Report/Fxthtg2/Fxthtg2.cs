using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.Fxthtg2
{
    [Serializable]
    public class Fxthtg2Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<Fxthtg2Row> LsData { get; set; }

        public Fxthtg2Model()
        {
            LsData = new List<Fxthtg2Row>();
        }
    }

    public class Fxthtg2Row
    {
        public int Stt { get; set; }
        public int Tag { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public string Ma2 { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
    }
}
