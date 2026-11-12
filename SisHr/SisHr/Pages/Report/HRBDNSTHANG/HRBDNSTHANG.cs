using System;
using System.Collections.Generic;

namespace SisHr.Pages.Report.HRBDNSTHANG
{
    [Serializable]
    public class HRBDNSTHANGModel
    {
        public int Nam { get; set; }
        public string Ma_dvcs { get; set; }
        public List<HRBDNSTHANGRow> LsData { get; set; }

        public HRBDNSTHANGModel()
        {
            LsData = new List<HRBDNSTHANGRow>();
        }
    }

    public class HRBDNSTHANGRow
    {
        public int Thang { get; set; }
        public int Ns_dauthang { get; set; }
        public int Ns_moi { get; set; }
        public int Ns_nghi { get; set; }
        public int Ns_cuoithang { get; set; }
    }
}
