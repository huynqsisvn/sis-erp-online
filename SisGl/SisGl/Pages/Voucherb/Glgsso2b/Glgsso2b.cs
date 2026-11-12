using System;
using System.Collections.Generic;

namespace SisGl.Pages.Voucherb.Glgsso2b
{
    [Serializable]
    public class Glgsso2bModel
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Ma_dvcs { get; set; }
        public List<Glgsso2b> LsData { get; set; }
        public Glgsso2bModel()
        {
            LsData = new List<Glgsso2b>();
        }
    }

    public class Glgsso2b
    {
        public int Stt { get; set; }
        public string So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public string Stt_ctgs { get; set; }
        public string Dien_giai { get; set; }
        public string No_co { get; set; }
        public string No_co_browse { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_ct { get; set; }
        public decimal T_n { get; set; }
        public decimal T_c { get; set; }
        public decimal T_n_nt { get; set; }
        public decimal T_c_nt { get; set; }
        public int Tag { get; set; }
    }
}
