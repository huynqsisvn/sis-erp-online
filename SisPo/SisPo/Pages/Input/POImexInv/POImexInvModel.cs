using System;
using System.ComponentModel.DataAnnotations;

namespace SisPo.Pages.Input.POImexInv
{
    public class BkhddvtctModel
    {
        public int? row_id { get; set; }
        public string ws_id { get; set; }
        public string stt_rec { get; set; }
        public string ma_ct { get; set; }
        public string kh_mau_hd0 { get; set; }
        public string so_seri0 { get; set; }
        public DateTime? ngay_ct0 { get; set; }
        public string so_ct0 { get; set; }
        public string mst_mua { get; set; }
        public string ten_dv_mua { get; set; }
        public string mst_ban { get; set; }
        public string ten_dv_ban { get; set; }
        public string ma_cqt { get; set; }
        public string dien_giai { get; set; }
        public decimal? t_so_luong { get; set; }
        public string ma_nt { get; set; }
        public decimal? ty_gia { get; set; }
        public decimal? t_tien_nt { get; set; }
        public decimal? t_tien { get; set; }
        public string ma_thue { get; set; }
        public decimal? thue_suat { get; set; }
        public decimal? t_thue_nt { get; set; }
        public decimal? t_thue { get; set; }
        public decimal? t_tt_nt { get; set; }
        public decimal? t_tt { get; set; }
        public decimal? t_ck_nt { get; set; }
        public decimal? t_ck { get; set; }
        public int? han_tt { get; set; }
        public DateTime? date { get; set; }
        public string time { get; set; }
        public int? user_id { get; set; }
        public DateTime? date0 { get; set; }
        public string time0 { get; set; }
        public int? user_id0 { get; set; }
        public string status { get; set; }
        public int? sua_tien { get; set; }
        public string ma_dvcs { get; set; }
        public decimal? ty_giaf { get; set; }
        public string hd_thue { get; set; }
        public string id_hd { get; set; }
        public string tinh_trang_hd { get; set; }
        public string trang_thai_hd { get; set; }
        public string sysflag { get; set; }
        public string cks_cqt { get; set; }
        public DateTime? ngay_ks_cqt { get; set; }
        public string cks_nguoi_ban { get; set; }
        public DateTime? ngay_ks_ban { get; set; }
        public string dia_chi_ban { get; set; }
        public string dien_thoai_ban { get; set; }
        public string fax_ban { get; set; }
        public string e_mail_ban { get; set; }
        public string tk_nh_ban { get; set; }
        public string ten_tk_nh_ban { get; set; }
        public string ma_kh_mua { get; set; }
        public string nguoi_mua { get; set; }
        public string dia_chi_mua { get; set; }
        public string dien_thoai_mua { get; set; }
        public string fax_mua { get; set; }
        public string e_mail_mua { get; set; }
        public string tk_nh_mua { get; set; }
        public string ten_tk_nh_mua { get; set; }
        public string loai_hd { get; set; }
        public string hinh_thuc_tt { get; set; }
        public string doc_tien { get; set; }
        public string ghi_chu { get; set; }
        public string noi_dung_loi { get; set; }
        public string ghi_chu_loi { get; set; }
        public decimal? t_tien_nt0 { get; set; }
        public decimal? t_tien0 { get; set; }
        public string ttxly { get; set; }
        public DateTime? ngay_td_tt_cqt { get; set; }
        public DateTime? ngay_ph_td_tt { get; set; }
        public string hash_inv { get; set; }
        public int? stt { get; set; }
        public string ma_vt_inv { get; set; }
        public string ten_vt_inv { get; set; }
        public string dvt_inv { get; set; }
        public string tinh_chat { get; set; }
        public decimal? so_luong { get; set; }
        public decimal? gia_nt0 { get; set; }
        public decimal? gia0 { get; set; }
        public decimal? tien_nt0 { get; set; }
        public decimal? tien0 { get; set; }
        public decimal? tl_ck { get; set; }
        public decimal? ck_nt { get; set; }
        public decimal? ck { get; set; }
        public decimal? thue_suati { get; set; }
        public decimal? thue_nt { get; set; }
        public decimal? thue { get; set; }
        public string ma_thue_i { get; set; }
        public string ma_vt { get; set; }
        public string dvt1 { get; set; }
        public string ma_kh { get; set; }
        public string inv_guid { get; set; }
        public string stt_rec_pn { get; set; }
    }

    public class CaptchaInfo
    {
        public string Captcha { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string State { get; set; }

        public CaptchaInfo()
        {
            State = "OK";
        }

        public CaptchaInfo(string captcha, string key, string value)
        {
            Captcha = captcha;
            Key = key;
            Value = value;
            State = "OK";
        }

        public CaptchaInfo(string error)
        {
            State = "ERROR";
            Value = error;
        }
    }
}
