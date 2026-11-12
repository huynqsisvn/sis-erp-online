using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FxSocc1
{
    [Serializable]
    public class FxSocc1PrintModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public DateTime? Ngay_giam_ts { get; set; }
        public DateTime? Ngay_ms { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ten_bpcc { get; set; }
        public string Ten_dvcs { get; set; }
        public string Ten_cty { get; set; }
        public string Dia_chi { get; set; }

        public List<FxSocc1PrintRow> LsData { get; set; }

        /// <summary>Định dạng ngày mở sổ dd/MM/yyyy cho báo cáo</summary>
        public string Ngay_ms_Formatted
        {
            get
            {
                return Ngay_ms.HasValue ? Ngay_ms.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public FxSocc1PrintModel()
        {
            LsData = new List<FxSocc1PrintRow>();
        }
    }

    public class FxSocc1PrintRow
    {
        // ===== CH?NG T? (Column A, B, C) =====
        /// <summary>S? hi?u CCDC</summary>
        public string So_hieu_cc { get; set; }

        /// <summary>Ngày t?ng CCDC</summary>
        public DateTime? Ngay_tang_cc { get; set; }

        /// <summary>Lý do t?ng CCDC</summary>
        public string Ly_do_tang_cc { get; set; }

        // ===== CCDC T?NG (Column D, E, G, H, 1) =====
        /// <summary>Tên, ??c ?i?m, ký hi?u CCDC</summary>
        public string Ten_cc { get; set; }

        /// <summary>Tên ti?ng Anh</summary>
        public string Ten_cc2 { get; set; }

        /// <summary>N??c s?n xu?t / S?n xu?t</summary>
        public string Nuoc_sx { get; set; }

        /// <summary>Ngày mua/Vào s?</summary>
        public DateTime? Ngay_mua { get; set; }

        /// <summary>S? hi?u CCDC (l?p l?i)</summary>
        public string Ma_cc { get; set; }

        /// <summary>Mã CCDC g?c (ma_cc_me)</summary>
        public string Ma_cc_me { get; set; }

        /// <summary>Nguyên giá CCDC</summary>
        public decimal Nguyen_gia { get; set; }

        /// <summary>STT</summary>
        public int? Stt { get; set; }

        /// <summary>S? th? CCDC</summary>
        public string So_ct { get; set; }

        /// <summary>Ngày th? CCDC</summary>
        public DateTime? Ngay_ct { get; set; }

        // ===== PHÂN B? CCDC (Column 2, 3, 4) =====
        /// <summary>Lo?i phân b? (%)</summary>
        public string Loai_pb { get; set; }

        /// <summary>T? l? phân b? (%)</summary>
        public decimal Tl_pb { get; set; }

        /// <summary>M?c phân b?</summary>
        public string Muc_pb { get; set; }

        /// <summary>Tài kho?n phân b?</summary>
        public string Tk_pb { get; set; }

        /// <summary>Tài kho?n chi phí</summary>
        public string Tk_cp { get; set; }

        /// <summary>B? ph?n ?ã tính kh?u hao</summary>
        public string Bp_da_tinh_kh { get; set; }

        /// <summary>M?c kh?u hao</summary>
        public string Muc_kh { get; set; }

        /// <summary>S? k? kh?u hao</summary>
        public int So_ky_kh { get; set; }

        /// <summary>T? l? kh?u hao</summary>
        public decimal Tl_kh { get; set; }

        /// <summary>Giá tr? ?ã kh?u hao</summary>
        public decimal Gt_da_kh { get; set; }

        /// <summary>Giá tr? còn l?i</summary>
        public decimal Gt_con_lai { get; set; }

        // ===== GHI GI?M CCDC (Column I, K, L) =====
        /// <summary>Ch?ng t? gi?m</summary>
        public string Chung_tu_giam { get; set; }

        /// <summary>S? ch?ng t? gi?m</summary>
        public string So_ct_giam { get; set; }

        /// <summary>Ngày gi?m CCDC</summary>
        public DateTime? Ngay_giam { get; set; }

        /// <summary>Lý do gi?m CCDC</summary>
        public string Ly_do_giam_cc { get; set; }

        /// <summary>Mã lý do gi?m</summary>
        public string Ma_giam_cc { get; set; }

        /// <summary>Tên lý do gi?m</summary>
        public string Ten_giam { get; set; }

        /// <summary>Tên lý do gi?m ti?ng Anh</summary>
        public string Ten_giam2 { get; set; }

        // ===== THÔNG TIN H?TR? =====
        /// <summary>Tháng tính kh?u hao</summary>
        public int Thang_tinh_kh { get; set; }

        /// <summary>N?m tính kh?u hao</summary>
        public int Nam_tinh_kh { get; set; }

        /// <summary>Nhóm CCDC</summary>
        public string Nh_cc { get; set; }

        /// <summary>Nhóm CCDC 1</summary>
        public string Nh_cc1 { get; set; }

        /// <summary>Nhóm CCDC 2</summary>
        public string Nh_cc2 { get; set; }

        /// <summary>Nhóm CCDC 3</summary>
        public string Nh_cc3 { get; set; }

        /// <summary>Lo?i CCDC</summary>
        public string Loai_cc { get; set; }

        /// <summary>Ghi chú</summary>
        public string Ghi_chu { get; set; }

        /// <summary>B? ph?n s? d?ng</summary>
        public string Ma_bpcc { get; set; }

        /// <summary>??n v? công s?</summary>
        public string Ma_dvcs { get; set; }
    }
}
