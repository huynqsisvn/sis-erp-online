using System;

namespace SisFa.Pages.Input.FaTinhKH
{
    /// <summary>
    /// Model ánh x? t? b?ng CTKHTS — k?t qu? tr? v? t? SP Fatinhkh.
    /// </summary>
    public class Ctkhts
    {
        public int     Stt        { get; set; }
        public string  So_the_ts  { get; set; } = "";
        public string  Ten_ts     { get; set; } = "";
        public string  Ten_ts2    { get; set; } = "";
        public string  Ma_bpts    { get; set; } = "";
        public string  Ten_bpts   { get; set; } = "";
        public int     Nam        { get; set; }
        public int     Ky         { get; set; }
        public string  Ma_dvcs    { get; set; } = "";
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh   { get; set; }
        public decimal Gt_cl      { get; set; }
        public decimal Gt_kh_ky   { get; set; }
        public string  Tk_ts      { get; set; } = "";
        public string  Tk_kh      { get; set; } = "";
        public string  Tk_cp      { get; set; } = "";
        public string  Ma_phi     { get; set; } = "";
        public string  Ma_bpht    { get; set; } = "";
        public string  Ma_px      { get; set; } = "";
        public string  Ma_vv      { get; set; } = "";
        public string  Ma_nv      { get; set; } = "";
    }
}
