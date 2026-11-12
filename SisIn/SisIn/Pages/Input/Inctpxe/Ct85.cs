using System;

namespace SisIn.Pages.Input.Inctpxe
{
    /// <summary>
    /// Model cho b?ng ct85 - Chi ti?t phi?u xu?t ?i?u chuy?n (Detail)
    /// </summary>
    public class Ct85
    {
        public String Stt_rec { get; set; }
        public String Stt_rec0 { get; set; }
        public Int32 Stt { get; set; }
        public String Ma_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public String So_ct { get; set; }
        
        // Thông tin v?t t?
        public String Ma_vt { get; set; }
        public String Tk_vt { get; set; }                       // Tài kho?n v?t t? (n?)
        public String Ma_nx_i { get; set; }                     // Tài kho?n nh?p kho (có)
        public String Ma_vv_i { get; set; }                     // Mã v? vi?c
        
        // S? l??ng và ??n v? tính
        public Decimal So_luong { get; set; }
        public String Dvt1 { get; set; }
        public Decimal He_so1 { get; set; }
        public Decimal So_luong1 { get; set; }
        
        // Giá và ti?n
        public Decimal Gia_nt { get; set; }
        public Decimal Gia { get; set; }
        public Decimal Tien_nt { get; set; }
        public Decimal Tien { get; set; }
        
        // Mã t? do
        public String Ma_td_i { get; set; }
        public String Ma_td2_i { get; set; }
        public String Ma_td3_i { get; set; }
        
        // Thông tin phi?u nh?p liên quan
        public String Stt_rec_pn { get; set; }
        public String Stt_rec0pn { get; set; }
        
        // Ngày và s? l??ng t? do
        public DateTime? Ngay_td1_i { get; set; }
        public Decimal Sl_td1_i { get; set; }
        public Decimal Sl_td2_i { get; set; }
        public Decimal Sl_td3_i { get; set; }
        
        public String Gc_td1_i { get; set; }
        public String Gc_td2_i { get; set; }
        public String Gc_td3_i { get; set; }
        
        public String Tk_thue_i { get; set; }
        
        public String Ma_sp { get; set; }
        public Decimal Sl_sp { get; set; }
        public String Ten_sp { get; set; }
        public String Ten_sp2 { get; set; }

        public String Ma_phi_i { get; set; }
        public String So_lsx_i { get; set; }
        public String So_dh_i { get; set; }
        
        // Lot và h?n s? d?ng
        public String Lot_nbr { get; set; }
        public DateTime? Han_sd_lot { get; set; }
        
        public String Ma_nvhh { get; set; }
        public String Loc_id { get; set; }
        
        // ??n v? tính 2
        public String Dvt2 { get; set; }
        public Decimal So_luong2 { get; set; }
        
        // B? ph?n, h?p ??ng, khu
        public String Ma_bpht_i { get; set; }
        public String Ma_hd_i { get; set; }
        public String Ma_ku_i { get; set; }
        
        // Ngày và s? l??ng b? sung
        public DateTime? Ngay_s1_i { get; set; }
        public DateTime? Ngay_s2_i { get; set; }
        public String Gc_s1_i { get; set; }
        public String Gc_s2_i { get; set; }
        public Decimal Sl_s1_i { get; set; }
        public Decimal Sl_s2_i { get; set; }
        
        public String Ma_s1_i { get; set; }
        public String Ma_s2_i { get; set; }
        
        public String Gc0_i { get; set; }
        public String Ma_ns_i { get; set; }
        public String Ma_tb_i { get; set; }
        public String Ma_hdm_i { get; set; }
        public String So_ct_tc { get; set; }
        public DateTime? Ngay_ct_tc { get; set; }
        public String Ma_px_i { get; set; }
        public Int32 Stt_dong { get; set; }
        public String Ma_td4_i { get; set; }        
        public String Ma_lo { get; set; }        
        public String Ten_vt { get; set; }
        public String Ten_vt2 { get; set; }
        public String Dvt { get; set; }
        public Int32 Vt_ton_kho { get; set; }
        public Int32 Gia_ton { get; set; }
        public Decimal Ton13 { get; set; }
        public DateTime? Ngay_hh { get; set; }
        public Int32 Sua_tk_vt { get; set; }
    }
}
