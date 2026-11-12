using System;

namespace SisFx.Pages.Input.FxSuaKH
{
    /// <summary>
    /// Model ánh xạ từ bảng CTKHCC — dùng cho điều chỉnh phân bổ công cụ dụng cụ.
    /// </summary>
    public class FxSuaKHItem
    {
        public int     Stt        { get; set; }
        public string  Ma_cc_me   { get; set; } = "";
        public string  Ma_cc   { get; set; } = "";
        public string  Ten_cc     { get; set; } = "";
        public string  Ten_cc2    { get; set; } = "";
        public string  Ma_bpcc    { get; set; } = "";
        public int     Nam        { get; set; }
        public int     Ky         { get; set; }
        public string  Ma_dvcs    { get; set; } = "";
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh   { get; set; }
        public decimal Gt_cl      { get; set; }
        /// <summary>Giá trị còn lại đầu kỳ (gt_cl0) — dùng cho công thức tính gt_cl2</summary>
        public decimal Gt_cl0     { get; set; }
        /// <summary>Giá trị phân bổ kỳ gốc (readonly)</summary>
        public decimal Gt_kh_ky   { get; set; }
        /// <summary>Giá trị điều chỉnh phân bổ (editable)</summary>
        public decimal Gt_kh_dc   { get; set; }
        /// <summary>Giá trị còn lại 2 (sau điều chỉnh): gt_cl2 = gt_cl0 - gt_kh_dc + gt_kh_ky</summary>
        public decimal Gt_cl2     { get; set; }
        public string  Dien_giai  { get; set; } = "";
        public int     Sua_kh     { get; set; }
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
