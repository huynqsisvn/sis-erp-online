using System;

namespace SisIn.Pages.Input.Inctpxe
{
    /// <summary>
    /// Model cho b?ng ph85 - Phi?u xu?t ?i?u chuy?n (Header)
    /// </summary>
    public class Ph85
    {
        public String Stt_rec { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_nk { get; set; }
        public String Ma_gd { get; set; }
        public Int32 Px_gia_dd { get; set; }                    // numeric(1,0)
        
        public DateTime? Ngay_ct { get; set; }
        public DateTime? Ngay_lct { get; set; }
        public String So_ct { get; set; }
        
        public String So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
        
        // Thông tin l?nh ?i?u ??ng
        public String So_lenhdd { get; set; }
        public DateTime? Ngay_lenhdd { get; set; }
        public String Cua_lenhdd { get; set; }
        public String Viec_lenhdd { get; set; }
        public String Mst_lenhdd { get; set; }
        public String Hd_lenhdd { get; set; }                   // varchar(18)
        public String Phuong_tien_lenhdd { get; set; }
        
        public String So_dh { get; set; }
        public String Ong_ba { get; set; }
        public String Dia_chi { get; set; }
        public String Dien_giai { get; set; }
        
        // Kho xu?t và kho nh?p
        public String Ma_kho { get; set; }
        public String Ma_khon { get; set; }
        
        // T?ng s? l??ng
        public Decimal T_so_luong { get; set; }
        public Decimal T_soluong1 { get; set; }
        
        // Ngo?i t? và t? giá
        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }
        public Decimal T_tien_nt { get; set; }
        public Decimal T_tien { get; set; }
        
        // Audit fields
        public DateTime? Date { get; set; }
        public String Time { get; set; }
        public String User_id { get; set; }
        public DateTime? Date0 { get; set; }
        public String Time0 { get; set; }
        public String User_id0 { get; set; }
        
        public String Status { get; set; }                     // char(1)
        public String So_lo1 { get; set; }
        public String Ma_bpht { get; set; }
        public String Loai_ct { get; set; }                    // char(2)
        public String So_lsx { get; set; }
        
        // Mã t? do
        public String Ma_td1 { get; set; }
        public String Ma_td2 { get; set; }
        public String Ma_td3 { get; set; }
        
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        
        public Decimal Sl_td1 { get; set; }
        public Decimal Sl_td2 { get; set; }
        public Decimal Sl_td3 { get; set; }
        
        public String Gc_td1 { get; set; }
        public String Gc_td2 { get; set; }
        public String Gc_td3 { get; set; }
        
        public String Ma_hd { get; set; }
        public String Ma_ku { get; set; }
        public Int32 Sua_tien { get; set; }                     // numeric(1,0)
        
        public DateTime? Ngay_s1 { get; set; }
        public DateTime? Ngay_s2 { get; set; }
        public String Gc_s1 { get; set; }
        public String Gc_s2 { get; set; }
        public Decimal Sl_s1 { get; set; }
        public Decimal Sl_s2 { get; set; }
        
        public String Ma_s1 { get; set; }
        public String Ma_s2 { get; set; }
        public String Ma_tb { get; set; }
        public String Ma_hdm { get; set; }
        
        public String Ma_dvcs { get; set; }
        public String Ma_px { get; set; }
        public String Ma_qs { get; set; }
        public Decimal Ty_giaf { get; set; }
        
        public String So_pn { get; set; }
        public Int32 Row_id { get; set; }                         // IDENTITY
        
        public String Ws_id { get; set; }                      // char(1)
        public String Ghi_chu0 { get; set; }
        public Int32 Sl_in { get; set; }                        // S? l?n in, default 0
        public String Hd_thue { get; set; }                    // char(1)
        
        // Extended fields for UI
        public String Ten_kho { get; set; }
        public String Ten_kho_n { get; set; }
        public String Ten_nt { get; set; }
    }
}
