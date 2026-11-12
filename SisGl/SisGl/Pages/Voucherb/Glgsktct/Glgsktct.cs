using System;
using System.Collections.Generic;

namespace SisGl.Pages.Voucherb.Glgsktct
{
    [Serializable]
    public class GlgsktctModel
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Ma_dvcs { get; set; }
        public List<Glgsktct> LsData { get; set; }
        public GlgsktctModel()
        {
            LsData = new List<Glgsktct>();
        }
    }

    public class Glgsktct
    {
        public int Stt { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_lct { get; set; }
        public string Dien_giai { get; set; }
        public string Tk { get; set; }
        public string Ten_tk { get; set; }
        public string Tk_du { get; set; }
        public string Ten_tk_du { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public string Ma_nt { get; set; }
        public decimal Ty_gia { get; set; }
        public string Ma_kh { get; set; }
        public string Ten_kh { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_ct { get; set; }
        public string Ma_qs { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public string So_lo { get; set; }
        public string Stt_ctgs { get; set; }
        public string No_co { get; set; }
    }
}
