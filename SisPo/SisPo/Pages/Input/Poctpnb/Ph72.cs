using System;
using System.ComponentModel.DataAnnotations;

namespace SisPo.Pages.Input.Poctpnb
{
    public class Ph72
    {
        public String Stt_rec { get; set; }
        public int Row_id { get; set; }
        public String Ws_id { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_nk { get; set; }
        public String Ma_gd { get; set; }
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public String So_ct { get; set; }
        public String So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public String So_ct0 { get; set; }
        public DateTime? Ngay_ct0 { get; set; }
        public String So_seri0 { get; set; }
        public String So_dh { get; set; }

        [Required]
        public String Ma_kh { get; set; }
        public String Ten_kh { get; set; }
        public String Ong_ba { get; set; }
        public String Dia_chi { get; set; }
        public String Ma_so_thue { get; set; }
        public String Dien_giai { get; set; }

        [Required]
        public String Ma_nx { get; set; }
        public int Sua_tien { get; set; }
        public Decimal T_so_luong { get; set; }
        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }
        public Decimal T_tien_nt { get; set; }
        public Decimal T_tien { get; set; }
        public Decimal T_tien_nt0 { get; set; }
        public Decimal T_tien0 { get; set; }
        public Decimal T_tien_nt1 { get; set; }
        public Decimal T_tien1 { get; set; }
        public Decimal T_cp_nt { get; set; }
        public Decimal T_cp { get; set; }
        public Decimal T_nk_nt { get; set; }
        public Decimal T_nk { get; set; }
        public String Tk_thue_nk { get; set; }
        public Decimal T_db_nt { get; set; }
        public Decimal T_db { get; set; }
        public String Tk_thue_db { get; set; }
        public String Tk_thue_no { get; set; }
        public String Ma_kh2 { get; set; }
        public String Tk_thue_co { get; set; }
        public String Ma_thue { get; set; }
        public Decimal Thue_suat { get; set; }
        public Decimal T_thue_nt { get; set; }
        public Decimal T_thue { get; set; }
        public Decimal T_tt_nt { get; set; }
        public Decimal T_tt { get; set; }
        public Decimal T_tc_nt { get; set; }
        public Decimal T_tc { get; set; }
        public Int32 Han_tt { get; set; }
        public DateTime? Date { get; set; }
        public String Time { get; set; }
        public String User_id { get; set; }
        public DateTime? Date0 { get; set; }
        public String Time0 { get; set; }
        public String User_id0 { get; set; }
        public String Status { get; set; }

        [Required]
        public String Ma_qs { get; set; }
        public Decimal Ty_giaf { get; set; }
        public Decimal Ty_gia_hq { get; set; }
        public Decimal Ty_gia_hqf { get; set; }
        public String Loai_pb { get; set; }
        public String Ma_kh_i { get; set; }
        public String Tk_i { get; set; }
        public Int32 Thue_nk_cp { get; set; }
        public Int32 Tao_pc { get; set; }
        public String Stt_rec_pc { get; set; }
        public String So_ct_pc { get; set; }
        public String Ma_ct_pc { get; set; }
        public String Ma_qs_pc { get; set; }        
        public String Ong_ba_pc { get; set; }
        public String Dien_giai_pc { get; set; }
        public String Ma_gd_pc { get; set; }
        public String Ma_dvcs { get; set; }
    }
}
