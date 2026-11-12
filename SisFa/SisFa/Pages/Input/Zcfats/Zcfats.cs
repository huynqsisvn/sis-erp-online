using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisFa.Pages.Input.Zcfats
{
    // 1. Model chính: Bảng DMTS
    public class Dmts
    {
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string So_hieu_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Dvt { get; set; }
        public string Nuoc_sx { get; set; }
        public decimal? Nam_sx { get; set; }
        public string Nh_ts { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public int? Tinh_kh { get; set; }
        public string Ma_tg_ts { get; set; }
        public DateTime? Ngay_mua { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal? So_ky_kh { get; set; }
        public decimal? Tien_cl { get; set; }
        public DateTime? Ngay_khc { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public string Kieu_kh { get; set; }
        public decimal? Ty_le_kh { get; set; }
        public decimal? Tong_sl { get; set; }
        public string Ma_bpts { get; set; }
        public string Ma_bpht { get; set; }
        public string Loai_pb { get; set; }

        // Các tài khoản
        public string Tk_ts { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }

        public decimal? Cong_suat { get; set; }
        public string Loai_ts { get; set; }
        public string Ts_kt { get; set; }
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_dc { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public string Ma_giam_ts { get; set; }
        public string Ly_do_giam { get; set; }
        public decimal? So_luong { get; set; }
        public string Ghi_chu { get; set; }
        public string Status { get; set; }
        public DateTime? Ngay_dvsd { get; set; }
        public string So_ct { get; set; }
        public string Ma_tra_cuu { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_px { get; set; }
        public string Search { get; set; }
        public decimal? Tl_kh_nhanh { get; set; }
        public DateTime? Ngay_kt_kh { get; set; }
        public decimal? So_ky_kh0 { get; set; }
        public string Ma_vv { get; set; }
        public DateTime? Ngay_bbbg { get; set; }
        public string So_bbbg { get; set; }
        public string Ly_do_thoi { get; set; }

        // Các trường tính toán tổng hợp (hiển thị trên grid danh sách)
        public decimal? Nguyen_gia { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public decimal? Gt_da_kh0 { get; set; }

        // Bổ sung các trường Mã tự do (nếu form có dùng)
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }

        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }

        // 2. Người sửa (Update mỗi lần lưu)
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
    }

    // 2. Model chi tiết: Bảng CTDMTS
    public class CtDmts
    {
        public int Id { get; set; }
        public string Ts0 { get; set; }
        public string So_the_ts { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public DateTime? Ngay_tg { get; set; }
        public string So_ct { get; set; }
        public string Ma_tg_ts { get; set; }

        // QUAN TRỌNG: Phải thêm dấu ? để tránh lỗi khi Grid trả về null hoặc khi dùng toán tử ??
        public decimal? Nguyen_gia { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_tang { get; set; }
        public decimal? Gt_giam { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public decimal? Gt_da_kh0 { get; set; }

        public string Dien_giai { get; set; }
        public decimal? So_ky { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public DateTime? Ngay_thoi_kh { get; set; }
        public string Ma_nv { get; set; }

        // Field hỗ trợ hiển thị STT trên lưới
        public int Stt { get; set; }
    }

    // 3. Model phụ tùng: Bảng CTCTTS
    public class CtCtTs
    {
        public string Ts0 { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ptkt { get; set; }
        public string Dvt { get; set; }

        // Thêm dấu ? cho số lượng/giá trị
        public decimal? So_luong { get; set; }
        public decimal? Gia_tri { get; set; }

        public string Status { get; set; }

        // Field hỗ trợ hiển thị STT
        public int Stt { get; set; }
    }

    // 4. Model bộ phận/tài khoản lịch sử: Bảng CTBPTS
    public class CtBpTs
    {
        public int Id { get; set; }
        public string So_the_ts { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public string Ma_bpts { get; set; }
        public string Tk_ts { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_bp { get; set; }
        public string Ghi_chu { get; set; }
    }
}