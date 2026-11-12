using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXTHECC
{
    /// <summary>
    /// Model chính truy?n cho ReportOptionComponent.
    /// Ch?a thông tin l?c, t?ng c?ng, danh sách brow (LsData),
    /// và d? li?u in chi ti?t t? SP [FxTheCc-Print1].
    /// </summary>
    [Serializable]
    public class FXTHECCModel
    {
        // ===== Thông tin l?c =====
        public int Nam { get; set; }
        public int Ky { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        // ===== T?ng c?ng (dùng cho brow) =====
        public decimal T_ngia1 { get; set; }
        public decimal T_gt_da_kh0 { get; set; }
        public decimal T_gt_cl0 { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public List<FXTHECCRow> LsData { get; set; }
        public List<FXTHECCTong> Tong { get; set; }
        public List<FXTHECCPrint1_TMtemp1> tMtemp1 { get; set; }
        public List<FXTHECCPrint1_Accessory> Accessories { get; set; }
        public List<FXTHECCPrint1_Info> tmpInfo { get; set; }
        public List<FXTHECCPrint1_Empty> tempEmpty { get; set; }

        public FXTHECCModel()
        {
            LsData = new List<FXTHECCRow>();
            Tong = new List<FXTHECCTong>();
            tMtemp1 = new List<FXTHECCPrint1_TMtemp1>();
            Accessories = new List<FXTHECCPrint1_Accessory>();
            tmpInfo = new List<FXTHECCPrint1_Info>();
            tempEmpty = new List<FXTHECCPrint1_Empty>();
        }
    }

    public class FXTHECCRow
    {
        public int Stt { get; set; }
        public bool Tag { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_cc { get; set; }
        public DateTime? Ngay_mua { get; set; }
        public string Ma_tg_cc { get; set; }
        public string Ten_tang { get; set; }
        public string Ten_tang2 { get; set; }
        public string So_bbbg { get; set; }
        public DateTime? Ngay_bbbg { get; set; }
        public string So_hieu_cc { get; set; }
        public string Nuoc_sx { get; set; }
        public int Nam_sx { get; set; }
        public string Ma_bpcc { get; set; }
        public DateTime? Ngay_dvsd { get; set; }
        public string Cc_kt { get; set; }
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_dc { get; set; }
        public decimal Sl_cc { get; set; }
        public decimal Ngia1 { get; set; }
        public decimal Gt_da_kh0 { get; set; }
        public decimal Gt_cl0 { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public DateTime? Ngay_giam1 { get; set; }
        public string Ma_giam_cc1 { get; set; }
        public string Ten_giam { get; set; }
        public string Ten_giam2 { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Loai_cc { get; set; }
    }

    public class FXTHECCTong
    {
        public decimal Ngia1 { get; set; }
        public decimal Gt_da_kh0 { get; set; }
        public decimal Gt_cl0 { get; set; }
        public decimal Gt_da_kh1 { get; set; }
    }

    // =========================================================
    // CÁC MODEL CHO IN (SP [FxTheCc-Print1])
    // =========================================================

    /// <summary>
    /// Result set 1 — #tMtemp1: L?ch s? ch?ng t? t?ng/gi?m và phân b? theo n?m.
    /// Output cu?i cùng: recid, tag, so_ct, ngay_ct, ngayct, thangct, namct,
    ///                    dien_giai, nam, ky, marks, nguyen_gia, gt_hm, cong_don
    /// </summary>
    public class FXTHECCPrint1_TMtemp1
    {
        public int Recid { get; set; }
        public int? Tag { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ngayct { get; set; }
        public string Thangct { get; set; }
        public string Namct { get; set; }
        public string Dien_giai { get; set; }
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Marks { get; set; }
        public decimal? Nguyen_gia { get; set; }
        public decimal? Gt_hm { get; set; }
        public decimal? Cong_don { get; set; }
    }

    /// <summary>
    /// Result set 2 — #qrcttmp: Linh ki?n ph? tùng kèm theo CCDC.
    /// </summary>
    public class FXTHECCPrint1_Accessory
    {
        public int Stt { get; set; }
        public string Ten_ptkt { get; set; }
        public string Dvt { get; set; }
        public decimal So_luong { get; set; }
        public decimal Gia_tri { get; set; }
    }

    /// <summary>
    /// Result set 3 — #tmpInfo: Thông tin chính c?a th? CCDC (header in).
    /// </summary>
    public class FXTHECCPrint1_Info
    {
        public int Stt { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngayct { get; set; }
        public string Ngay_ct { get; set; }
        public string Thang_ct { get; set; }
        public string Nam_ct { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ngay_mua { get; set; }
        public string Thang_mua { get; set; }
        public string Nam_mua { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public string Nuoc_sx { get; set; }
        public int Nam_sx { get; set; }
        public string So_hieu_cc { get; set; }
        public string Cong_suat { get; set; }
        public string Cc_kt { get; set; }
        public DateTime? Ngay_dvsd { get; set; }
        public int? Nam_dvsd { get; set; }
        public string Ngay_dc { get; set; }
        public string Thang_dc { get; set; }
        public string Nam_dc { get; set; }
        public string Ly_do_dc { get; set; }
        public decimal Tien_cl { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public string Ngay_giam { get; set; }
        public string Thang_giam { get; set; }
        public string Nam_giam { get; set; }
        public string Ly_do_giam { get; set; }
        public string So_ct_giam { get; set; }
    }

    /// <summary>
    /// Result set 4 — #tempEmpty: Dòng tr?ng dùng cho in b? sung.
    /// </summary>
    public class FXTHECCPrint1_Empty
    {
        public string Nothing { get; set; }
    }
}
