using System;
using System.Collections.Generic;

namespace SisFa.Pages.Input.FAbtpbts_Tao
{
    /// <summary>
    /// Model phiếu (master) cho kết quả tính phân bổ khấu hao — dùng cho lưới PH.
    /// Ánh xạ từ DataSet trả về bởi SP store_proc (menu wf.ct.09).
    /// Tương đương DataTable table2 (Tables[Count-2]) trong WinForm StartUp.cs.
    /// </summary>
    public class PhBtpbts
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
    /// Model chi tiết (CT) cho một dòng phân bổ khấu hao — dùng cho lưới CT.
    /// Tương đương DataTable table1 (Tables[Count-1]) trong WinForm StartUp.cs.
    /// </summary>
    public class CtBtpbts
    {
        public int     Stt        { get; set; }
        public string  So_the_ts  { get; set; } = "";
        public string  Ten_ts     { get; set; } = "";
        public string  Ma_bpts    { get; set; } = "";
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
        public string  Stt_rec    { get; set; } = "";
    }

    /// <summary>
    /// Model cho dialog Tạo bút toán (FrmTaobt trong WinForm).
    /// </summary>
    public class TaoBtModel
    {
        public string Ma_qs      { get; set; } = "";
        public string Ten_qs     { get; set; } = "";
        public string So_ct      { get; set; } = "";
        public string Dien_giai  { get; set; } = "";
        public string So_ct_format { get; set; } = "";
        public double So_ct_num  { get; set; }
        public int    So_ct_length { get; set; } = 10;
        public bool   IsOk       { get; set; }
    }

    /// <summary>
    /// Model dòng chi tiết báo cáo in — ánh xạ từ SP FAbtpbts_Tao_F7.
    /// SP trả về các cột từ ct00: stt, rn, ma_vv, ma_bpht, ma_phi, tk, tk_du,
    /// ps_no, ps_co, stt_rec, stt_rec0, ngay_ct, so_ct, dien_giai.
    /// </summary>
    public class FAbtpbts_Tao_F7Detail
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
    /// Model bao bọc dữ liệu in — truyền cho ShowReport.
    /// Pattern giống GLSNKMHModel, GLSNKBHModel trong SisGl.
    /// WinForm FrmIn tạo DataSet gồm: tbInfo (thông tin lọc) + tbDetail (dữ liệu SP F7).
    /// </summary>
    public class FAbtpbts_TaoReportModel
    {
        public string MaDVCS    { get; set; } = "";
        public int    Nam       { get; set; }
        public int    Ky        { get; set; }
        public int    Ky2       { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public List<FAbtpbts_Tao_F7Detail> LsData { get; set; } = new();
    }
}
