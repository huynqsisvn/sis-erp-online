using System;
using System.Collections.Generic;

namespace SisGl.Pages.Voucherb.Glgsso3
{
    [Serializable]
    public class Glgsso3Model
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Ma_dvcs { get; set; }
        public DateTime Ngay_lo { get; set; }
        public string So_lo { get; set; }
        public List<Glgsso3> LsData { get; set; }
        public Glgsso3Model()
        {
            LsData = new List<Glgsso3>();
        }
    }

    public class Glgsso3
    {
        public int Stt { get; set; }
        public bool Tag { get; set; }
        public string Stt_ctgs { get; set; }
        public int Nam { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public string So_lo { get; set; }
        public string Tk { get; set; }
        public string Tk_du { get; set; }
        public string Tk_no { get; set; }
        public string Tk_co { get; set; }
        public int No_co { get; set; }
        public string Dien_giai { get; set; }
        public decimal Tien { get; set; }
        public decimal Tien_nt { get; set; }
        public string No_co1 { get; set; }
        public string No_co2 { get; set; }
        public string Ten_tk { get; set; }
        public string Ten_tk2 { get; set; }
        public string Ten_tk_no { get; set; }
        public string Ten_tk_no2 { get; set; }
        public string Ten_tk_co { get; set; }
        public string Ten_tk_co2 { get; set; }
    }
}
