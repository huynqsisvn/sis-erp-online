using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisIn.Pages.Input.Inctpnd
{
    public class Ph74
    {
        public String Stt_rec { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_nk { get; set; }
        public String Ma_gd { get; set; }
        public Int32 Pn_gia_tb { get; set; }                    // numeric(1,0) thường dùng để lưu 0/1
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public String So_ct { get; set; }
        public String So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public String So_dh { get; set; }
        public String Ma_kh { get; set; }
        public String Ong_ba { get; set; }
        public String Dia_chi { get; set; }
        public String Dien_giai { get; set; }
        public Decimal T_so_luong { get; set; }
        public Decimal T_soluong1 { get; set; }
        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }
        public Decimal T_tien_nt { get; set; }
        public Decimal T_tien { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }                       // udt_time thường là char(8) kiểu HH:mm:ss
        public String User_id { get; set; }
        public DateTime? Date0 { get; set; }
        public TimeSpan Time0 { get; set; }
        public String User_id0 { get; set; }
        public String Status { get; set; }                     // char(1)
        public String So_lo1 { get; set; }
        public String Ma_bpht { get; set; }
        public String Loai_ct { get; set; }                    // char(2)
        public String So_lsx { get; set; }
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
        public Int32 Row_id { get; set; }                         // IDENTITY, không nullable
        public String Ws_id { get; set; }                      // char(1)
        public String Ghi_chu0 { get; set; }
        public String Hd_thue { get; set; }                    // char(1)
        public String Stt_rec_px { get; set; }
        public Int32 Loai_xnvl { get; set; }                    // tinyInt32
        public String Ma_qs_px { get; set; }
        public String So_ct_px { get; set; }
        public String Ma_ct_px { get; set; }
        public String Ma_kho_px { get; set; }
        public String Ma_nx_px { get; set; }
        public String Ong_ba_px { get; set; }
        public String Dien_giai_px { get; set; }

        public String Ten_kh { get; set; }

    }
}
