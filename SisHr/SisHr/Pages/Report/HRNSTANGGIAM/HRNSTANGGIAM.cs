using System;
using System.Collections.Generic;

namespace SisHr.Pages.Report.HRNSTANGGIAM
{
    [Serializable]
    public class HRNSTANGGIAMModel
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public string Ma_dvcs { get; set; }

        public List<HRNSTANGGIAMRow> LsData { get; set; }

        public HRNSTANGGIAMModel()
        {
            LsData = new List<HRNSTANGGIAMRow>();
        }
    }

    public class HRNSTANGGIAMRow
    {
        public int Stt { get; set; }
        public string Ma_bpns { get; set; }
        public string Ten_bpns { get; set; }
        public int Tang { get; set; }
        public int Giam { get; set; }
        public string Ghi_chu { get; set; }
    }
}
