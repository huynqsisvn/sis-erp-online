using System;

namespace SisFx.Pages.Input.FxTinhKH
{
    /// <summary>
    /// Model ánh xạ từ bảng CTKHCC — kết quả trả về từ SP Fxtinhkh.
    /// </summary>
    public class Ctkhcc
    {
        public int     Stt        { get; set; }
        public string  Ma_cc_me   { get; set; } = "";
        public string  Ten_cc     { get; set; } = "";
        public string  Ten_cc2    { get; set; } = "";
        public string  Ma_bpts    { get; set; } = "";
        public string  Ten_bpts   { get; set; } = "";
        public int     Nam        { get; set; }
        public int     Ky         { get; set; }
        public string  Ma_dvcs    { get; set; } = "";
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh   { get; set; }
        public decimal Gt_cl      { get; set; }
        public decimal Gt_kh_ky   { get; set; }
        public string  Tk_cc      { get; set; } = "";
        public string  Tk_kh      { get; set; } = "";
        public string  Tk_cp      { get; set; } = "";
        public string  Ma_phi     { get; set; } = "";
        public string  Ma_bpht    { get; set; } = "";
        public string  Ma_px      { get; set; } = "";
        public string  Ma_vv      { get; set; } = "";
        public string  Ma_nv      { get; set; } = "";
    }
}
