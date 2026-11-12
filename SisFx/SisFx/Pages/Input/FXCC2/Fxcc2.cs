using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisFx.Pages.Input.FXCC2
{
    // =========================================================
    // Model điều chuyển bộ phận sử dụng: Bảng CTBPCC
    // =========================================================
    public class CtBpcc
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public DateTime? Ngay_kh { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ten_bpcc { get; set; }
        public decimal? Sl_cc { get; set; }
        public string Tk_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_vv { get; set; }
        public string Ghi_chu { get; set; }
        public string Cc0 { get; set; }

        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
    }

    // =========================================================
    // Model tạm dừng phân bổ: Bảng DMCCTHOIKC
    // =========================================================
    public class CcThoiKc
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public DateTime? Ngay_thoi_kh1 { get; set; }
        public DateTime? Ngay_thoi_kh2 { get; set; }
        public string Ly_do_thoi { get; set; }
        public string Loai_thoi_kh { get; set; }

        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
    }

    // =========================================================
    // Model khai báo giảm cc (dùng cho một thẻ ctdmcc)
    // =========================================================
    public class EditGiamCc
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_giam_cc { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public string So_ct_giam { get; set; }
        public string Ly_do_giam { get; set; }
        public string Ten_tg_cc { get; set; }


        public DateTime? Date_giam0 { get; set; }
        public string Time_giam0 { get; set; }
        public int? User_id_giam0 { get; set; }
        public string User_name_giam0 { get; set; }
        public DateTime? Date_giam { get; set; }
        public string Time_giam { get; set; }
        public int? User_id_giam { get; set; }
        public string User_name_giam { get; set; }

        public DateTime? Date_thoi0 { get; set; }
        public string Time_thoi0 { get; set; }
        public int? User_id_thoi0 { get; set; }
        public int? User_name_thoi0 { get; set; }
        public DateTime? Date_thoi { get; set; }
        public string Time_thoi { get; set; }
        public int? User_id_thoi { get; set; }
        public int? User_name_thoi { get; set; }

        public string Ten_bpcc { get; set; }
        public decimal? Nguyen_gia { get; set; }
        public decimal? Sl_cc { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public string Ghi_chu { get; set; }
        public DateTime? Ngay_hong { get; set; }
    }

    // =========================================================
    // Model khai báo hỏng cc (dùng cho một thẻ ctdmcc)
    // =========================================================
    public class EditHongCc
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_hong_cc { get; set; }
        public DateTime? Ngay_hong { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public string So_ct_hong { get; set; }
        public string Ly_do_hong { get; set; }

        public DateTime? Date_hong0 { get; set; }
        public string Time_hong0 { get; set; }
        public int? User_id_hong0 { get; set; }
        public string User_name_hong0 { get; set; }
        public DateTime? Date_hong { get; set; }
        public string Time_hong { get; set; }
        public int? User_id_hong { get; set; }
        public string User_name_hong { get; set; }

        public DateTime? Date_thoi0 { get; set; }
        public string Time_thoi0 { get; set; }
        public int? User_id_thoi0 { get; set; }
        public DateTime? Date_thoi { get; set; }
        public string Time_thoi { get; set; }
        public int? User_id_thoi { get; set; }

        public string Ten_bpcc { get; set; }
        public decimal? Nguyen_gia { get; set; }
        public decimal? Sl_cc { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public string Ghi_chu { get; set; }
        public DateTime? Ngay_giam { get; set; }
    }

    // =========================================================
    // Model thôi phân bổ (dùng cho một thẻ ctdmcc)
    // =========================================================
    public class EditThoiCc
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string Ma_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public DateTime? Ngay_kh1 { get; set; }
        public string Ly_do_thoi { get; set; }
        public string So_ct { get; set; }

        public DateTime? Date_giam0 { get; set; }
        public string Time_giam0 { get; set; }
        public int? User_id_giam0 { get; set; }
        public string User_name_giam0 { get; set; }
        public DateTime? Date_giam { get; set; }
        public string Time_giam { get; set; }
        public int? User_id_giam { get; set; }
        public string User_name_giam { get; set; }

        public DateTime? Date_thoi0 { get; set; }
        public string Time_thoi0 { get; set; }
        public int? User_id_thoi0 { get; set; }
        public string User_name_thoi0 { get; set; }
        public DateTime? Date_thoi { get; set; }
        public string Time_thoi { get; set; }
        public int? User_id_thoi { get; set; }
        public string User_name_thoi { get; set; }

        public string Ten_bpcc { get; set; }
        public decimal? Nguyen_gia { get; set; }
        public decimal? Sl_cc { get; set; }
        public decimal? Gt_da_kh { get; set; }
        public decimal? Gt_cl { get; set; }
        public decimal? Gt_kh_ky { get; set; }
        public string Ghi_chu { get; set; }
    }

    // =========================================================
    // Helper item cho combobox
    // =========================================================
    public class Fxcc2ListItem
    {
        public string Ma_item { get; set; }
        public string Ten_item { get; set; }
    }
}
