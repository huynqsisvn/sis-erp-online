using System;

namespace SisFa.Pages.Input.FaSuaKH
{
    /// <summary>
    /// Model ánh x? t? b?ng CTKHTS — k?t qu? tr? v? t? SP ?i?u ch?nh kh?u hao.
    /// Dùng chung cho l??i danh sách và form s?a (F3).
    /// </summary>
    public class FaSuaKHItem
    {
        public int    Stt        { get; set; }
        public string So_the_ts  { get; set; } = "";
        public string Ten_ts     { get; set; } = "";
        public string Ten_ts2    { get; set; } = "";
        public string Ma_bpts    { get; set; } = "";
        public string Ten_bpts   { get; set; } = "";
        public int    Nam        { get; set; }
        public int    Ky         { get; set; }
        public string Ma_dvcs    { get; set; } = "";
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh  { get; set; }
        public decimal Gt_cl     { get; set; }
        /// <summary>Giá tr? còn l?i ??u k? (gt_cl0) — dùng cho công th?c tính gt_cl2</summary>
        public decimal Gt_cl0    { get; set; }
        /// <summary>Giá tr? kh?u hao k?</summary>
        public decimal Gt_kh_ky  { get; set; }
        /// <summary>Giá tr? ?i?u ch?nh kh?u hao</summary>
        public decimal Gt_kh_dc  { get; set; }
        /// <summary>Giá tr? còn l?i 2 (sau ?i?u ch?nh): gt_cl2 = gt_cl0 - gt_kh_dc + gt_kh_ky</summary>
        public decimal Gt_cl2    { get; set; }
        public int    Id_ma_nv   { get; set; }
        public string Ma_nv      { get; set; } = "";
        public string Ten_nv     { get; set; } = "";
        public string Dien_giai  { get; set; } = "";
        public int    Sua_kh     { get; set; }
        public string Tk_ts      { get; set; } = "";
        public string Tk_kh      { get; set; } = "";
        public string Tk_cp      { get; set; } = "";
        public string Ma_phi     { get; set; } = "";
        public string Ma_bpht    { get; set; } = "";
        public string Ma_px      { get; set; } = "";
        public string Ma_vv      { get; set; } = "";
    }

    /// <summary>
    /// Model dùng cho ch?c n?ng xóa nhi?u dòng — ch?a khóa chính composite.
    /// </summary>
    public class FaSuaKHDeleteKey
    {
        public int    Nam        { get; set; }
        public int    Ky         { get; set; }
        public string So_the_ts  { get; set; } = "";
        public string Ma_bpts    { get; set; } = "";
        public int    Id_ma_nv   { get; set; }
    }
}
