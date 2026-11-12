using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisFx.Pages.Input.FXCC
{
    // =========================================================
    // 1. Model Master: Bảng DMCC (Danh mục CCDC mẹ)
    // =========================================================
    public class Dmcc
    {
        /// <summary>Chuỗi kiểu phân bổ dùng cho ComboBox ("1" hoặc "2"), không map DB.</summary>
        [NotMapped]
        public string Kieu_kh_str { get; set; } = "1";
        public string Ma_cc_me { get; set; }
        /// <summary>Không có trong bảng dmcc, chỉ có ở ctdmcc – bỏ qua khi Insert/Update.</summary>
        [NotMapped]
        public string Ma_cc { get; set; }
        public string Ten_cc { get; set; }
        public string So_hieu_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Dvt { get; set; }
        public string Nuoc_sx { get; set; }
        public decimal? Nam_sx { get; set; }
        public string Nh_cc { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public decimal? Tinh_kh { get; set; }
        public string Ma_tg_cc { get; set; }
        public DateTime? Ngay_mua { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal? So_ky_kh { get; set; }
        public decimal? Tien_cl { get; set; }
        public DateTime? Ngay_khc { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public decimal? Kieu_kh { get; set; }
        public decimal? Ty_le_kh { get; set; }
        public decimal? Tong_sl { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_bpht { get; set; }
        public decimal? Loai_pb { get; set; }
        public string Tk_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public decimal? Cong_suat { get; set; }
        public string Loai_cc { get; set; }
        public string Cc_kt { get; set; }
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_dc { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public string Ma_giam_cc { get; set; }
        public string Ly_do_giam { get; set; }
        public decimal? So_luong { get; set; }
        public string Ghi_chu { get; set; }
        public string Status { get; set; }

        // Các trường tổng hợp giá trị
        public decimal? Nguyen_gia { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public decimal? Gt_da_kh0 { get; set; }
        public decimal? Tl_kh_nhanh { get; set; }
        public DateTime? Ngay_kt_kh { get; set; }
        public decimal? So_ky_kh0 { get; set; }

        public string Ma_vv { get; set; }
        public string So_bbbg { get; set; }
        public DateTime? Ngay_bbbg { get; set; }
        public string Ly_do_thoi { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_px { get; set; }
        public string Search { get; set; }
        public DateTime? Ngay_dvsd { get; set; }
        public string So_ct { get; set; }
        public string Ma_tra_cuu { get; set; }
        public DateTime? Ngay_hong { get; set; }
        public string So_cth { get; set; }
        public string Ly_do_hong { get; set; }
        public DateTime? Ngay_ct { get; set; }

        // Trường tự do
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        public string Gc_td1 { get; set; }
        public string Gc_td2 { get; set; }
        public string Gc_td3 { get; set; }
        public decimal? Sl_td1 { get; set; }
        public decimal? Sl_td2 { get; set; }
        public decimal? Sl_td3 { get; set; }

        // Audit Tracking
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
        public DateTime? Date2 { get; set; }
        public string Time2 { get; set; }
        public int? User_id2 { get; set; }

        // Audit Giảm / Thôi / Tăng
        public DateTime? Date_giam { get; set; }
        public string Time_giam { get; set; }
        public int? User_id_giam { get; set; }
        public DateTime? Date_giam0 { get; set; }
        public string Time_giam0 { get; set; }
        public int? User_id_giam0 { get; set; }

        public DateTime? Date_thoi { get; set; }
        public string Time_thoi { get; set; }
        public int? User_id_thoi { get; set; }
        public DateTime? Date_thoi0 { get; set; }
        public string Time_thoi0 { get; set; }
        public int? User_id_thoi0 { get; set; }

        public DateTime? Date_tang { get; set; }
        public string Time_tang { get; set; }
        public int? User_id_tang { get; set; }
        public DateTime? Date_tang0 { get; set; }
        public string Time_tang0 { get; set; }
        public int? User_id_tang0 { get; set; }
    }

    // =========================================================
    // 2. Model Detail 1: Bảng CTDMCC (Chi tiết thẻ CCDC con)
    // =========================================================
    public class CtDmcc
    {
        public int Id { get; set; }
        public decimal? Cc0 { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_gs { get; set; }
        public string Ma_nv { get; set; }
        public decimal? Ky { get; set; }
        public decimal? Nam { get; set; }
        public decimal? Tang_giam { get; set; }
        public string Ma_tg_cc { get; set; }

        public decimal? Nguyen_gia { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_tang { get; set; }
        public decimal? Gt_giam { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public decimal? Gt_da_kh0 { get; set; }
        public decimal? Tien_cl { get; set; }
        public string Dien_giai { get; set; }

        public decimal? So_ky { get; set; }
        public DateTime? Ngay_tg { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public DateTime? Ngay_thoi_kh { get; set; }
        public decimal? Sl_cc { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ten_bpcc { get; set; }
        public string Ten_bpcc2 { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string Ma_px { get; set; }

        public string Ma_giam_cc { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public string Ly_do_thoi { get; set; }
        public string Ly_do_giam { get; set; }
        public string Ghi_chu { get; set; }
        public string So_ct_giam { get; set; }

        public DateTime? Ngay_hong { get; set; }
        public string Ly_do_hong { get; set; }
        public string Ma_hong_cc { get; set; }
        public string So_ct_hong { get; set; }
        public string So_ct_tc { get; set; }
        public DateTime? Ngay_ct_tc { get; set; }

        // Trường tự do
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        public string Gc_td1 { get; set; }
        public string Gc_td2 { get; set; }
        public string Gc_td3 { get; set; }
        public decimal? Sl_td1 { get; set; }
        public decimal? Sl_td2 { get; set; }
        public decimal? Sl_td3 { get; set; }

        // Audit Tracking
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }

        public DateTime? Date_giam { get; set; }
        public string Time_giam { get; set; }
        public int? User_id_giam { get; set; }
        public DateTime? Date_giam0 { get; set; }
        public string Time_giam0 { get; set; }
        public int? User_id_giam0 { get; set; }

        public DateTime? Date_tang { get; set; }
        public string Time_tang { get; set; }
        public int? User_id_tang { get; set; }
        public DateTime? Date_tang0 { get; set; }
        public string Time_tang0 { get; set; }
        public int? User_id_tang0 { get; set; }

        public DateTime? Date_thoi { get; set; }
        public string Time_thoi { get; set; }
        public int? User_id_thoi { get; set; }
        public DateTime? Date_thoi0 { get; set; }
        public string Time_thoi0 { get; set; }
        public int? User_id_thoi0 { get; set; }

        public DateTime? Date_hong { get; set; }
        public string Time_hong { get; set; }
        public int? User_id_hong { get; set; }
        public DateTime? Date_hong0 { get; set; }
        public string Time_hong0 { get; set; }
        public int? User_id_hong0 { get; set; }

        // Biến hỗ trợ hiển thị STT trên Grid (không map vào SQL)
        [NotMapped]
        public int Stt { get; set; }
    }

    // =========================================================
    // 3. Model Detail 2: Bảng CTCTCC (Phụ tùng kèm theo CCDC)
    // =========================================================
    public class CtCtcc
    {
        public decimal? Cc0 { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_ptkt { get; set; }
        public string Dvt { get; set; }
        public decimal? So_luong { get; set; }
        public decimal? Gia_tri { get; set; }
        public string Status { get; set; }

        public string So_ct_tc { get; set; }
        public DateTime? Ngay_ct_tc { get; set; }

        // Trường tự do & Trường S
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        public string Gc_td1 { get; set; }
        public string Gc_td2 { get; set; }
        public string Gc_td3 { get; set; }
        public decimal? Sl_td1 { get; set; }
        public decimal? Sl_td2 { get; set; }
        public decimal? Sl_td3 { get; set; }

        public DateTime? Ngay_s1 { get; set; }
        public DateTime? Ngay_s2 { get; set; }
        public string Gc_s1 { get; set; }
        public string Gc_s2 { get; set; }
        public decimal? Sl_s1 { get; set; }
        public decimal? Sl_s2 { get; set; }
        public string Ma_s1 { get; set; }
        public string Ma_s2 { get; set; }

        // Audit Tracking
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }

        // Biến hỗ trợ hiển thị STT trên Grid (không map vào SQL)
        [NotMapped]
        public int Stt { get; set; }
    }
}