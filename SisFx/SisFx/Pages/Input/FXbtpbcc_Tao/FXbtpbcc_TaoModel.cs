using System;
using System.Collections.Generic;

namespace SisFx.Pages.Input.FXbtpbcc_Tao
{
    /// <summary>
    /// Model phi?u (master) cho k?t qu? t?o bút toán phân b? CCDC — l??i PH.
    /// T??ng ???ng DataTable table2 (Tables[Count-2]) trong WinForm StartUp.cs.
    /// </summary>
    public class PhBtpbcc
    {
        public int     Stt       { get; set; }
        public bool    Tag       { get; set; }
        public int     Nam       { get; set; }
        public int     Ky        { get; set; }
        public string  Ma_dvcs   { get; set; } = "";
        public string  Ten_dvcs  { get; set; } = "";
        public string  Tk_kh     { get; set; } = "";
        public string  Tk_cp     { get; set; } = "";
        public decimal Gt_kh_ky  { get; set; }
        public string  Ma_qs     { get; set; } = "";
        public string  So_ct     { get; set; } = "";
        public string  Dien_giai { get; set; } = "";
        public string  Stt_rec   { get; set; } = "";
    }

    /// <summary>
    /// Model chi ti?t (CT) cho m?t dòng phân b? CCDC — l??i CT.
    /// T??ng ???ng DataTable table1 (Tables[Count-1]) trong WinForm StartUp.cs.
    /// </summary>
    public class CtBtpbcc
    {
        public int     Stt        { get; set; }
        public string  Ma_cc_me   { get; set; } = "";
        public string  Ten_cc     { get; set; } = "";
        public string  Ma_bpcc    { get; set; } = "";
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
        public string  Stt_rec    { get; set; } = "";
    }

    /// <summary>
    /// Model cho dialog T?o bút toán (FrmTaobt trong WinForm).
    /// </summary>
    public class TaoBtModel
    {
        public string Ma_qs        { get; set; } = "";
        public string Ten_qs       { get; set; } = "";
        public string So_ct        { get; set; } = "";
        public string Dien_giai    { get; set; } = "";
        public string So_ct_format { get; set; } = "";
        public double So_ct_num    { get; set; }
        public int    So_ct_length { get; set; } = 10;
        public bool   IsOk         { get; set; }
    }

    /// <summary>
    /// Model dòng chi ti?t báo cáo in — ánh x? t? SP FXbtpbcc_Tao_F7.
    /// </summary>
    public class FXbtpbcc_Tao_F7Detail
    {
        public int?     Stt       { get; set; }
        public int?     Rn        { get; set; }
        public string   Ma_vv     { get; set; } = "";
        public string   Ma_bpht   { get; set; } = "";
        public string   Ma_phi    { get; set; } = "";
        public string   Tk        { get; set; } = "";
        public string   Tk_du     { get; set; } = "";
        public decimal  Ps_no     { get; set; }
        public decimal  Ps_co     { get; set; }
        public string   Stt_rec   { get; set; } = "";
        public string   Stt_rec0  { get; set; } = "";
        public DateTime? Ngay_ct  { get; set; }
        public string   So_ct     { get; set; } = "";
        public string   Dien_giai { get; set; } = "";
    }

    /// <summary>
    /// Model bao b?c d? li?u in — truy?n cho ReportOptionComponent.
    /// WinForm FrmIn t?o DataSet: tbInfo (thông tin l?c) + tbDetail (SP F7).
    /// </summary>
    public class FXbtpbcc_TaoReportModel
    {
        public string   MaDVCS   { get; set; } = "";
        public int      Nam      { get; set; }
        public int      Ky       { get; set; }
        public int      Ky2      { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public List<FXbtpbcc_Tao_F7Detail> LsData { get; set; } = new();
    }
}
