using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.Apctpn1
{
    public class Ph31
    {
        public String Ma_dvcs { get; set; }
        public String Stt_rec { get; set; }

        public Int32 Row_id { get; set; }                     // IDENTITY → không nullable

        public String Ws_id { get; set; }                  // char(1)

        public String Ma_ct { get; set; }
        public String Ma_nk { get; set; }
        public String Ma_gd { get; set; }

        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }

        public String So_dh { get; set; }
        public String So_ct { get; set; }
        public String So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }

        public String Ma_kh { get; set; }
        public String Ong_ba { get; set; }
        public String Dia_chi { get; set; }

        public String Ma_nx { get; set; }                  // tài khoản nợ/có tổng hợp
        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }

        public Decimal T_tien_nt { get; set; }
        public Decimal T_tien { get; set; }

        public Decimal T_thue_nt { get; set; }
        public Decimal T_thue { get; set; }

        public String Tk_thue_no { get; set; }             // tài khoản thuế đầu vào
        public String Ma_kh2 { get; set; }

        public Decimal Thue_suat { get; set; }             // % thuế suất
        public Int32 So_hd_gtgt { get; set; }              // numeric(2,0) → số hóa đơn GTGT

        public Decimal T_tt_nt { get; set; }               // tổng thanh toán ngoại tệ
        public Decimal T_tt { get; set; }                  // tổng thanh toán VND

        public Decimal T_cp { get; set; }                  // tổng chi phí
        public Decimal T_cp_nt { get; set; }

        public Int32 Han_tt { get; set; }                  // hạn thanh toán (ngày) numeric(3,0)

        public DateTime? Ngay_ct0 { get; set; }             // ngày chứng từ gốc
        public String So_seri0 { get; set; }
        public String So_ct0 { get; set; }

        public DateTime? Date { get; set; }
        public String? Time { get; set; }
        public String User_id { get; set; }

        public DateTime? Date0 { get; set; }
        public String? Time0 { get; set; }
        public String User_id0 { get; set; }

        public String Status { get; set; }                 // char(1)

        public String So_lo1 { get; set; }
        public String Ma_bpht { get; set; }
        public String Loai_ct { get; set; }                // char(2)

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
        public Int32 Sua_tien { get; set; }                 // numeric(1,0)

        public DateTime? Ngay_s1 { get; set; }
        public DateTime? Ngay_s2 { get; set; }
        public String Gc_s1 { get; set; }
        public String Gc_s2 { get; set; }
        public Decimal Sl_s1 { get; set; }
        public Decimal Sl_s2 { get; set; }

        public String Ma_s1 { get; set; }
        public String Ma_s2 { get; set; }

        public String Dien_giai { get; set; }
        public String Ma_tb { get; set; }
        public String Ma_hdm { get; set; }

        public String Ma_px { get; set; }
        public String Ma_qs { get; set; }
        public String Ma_thck { get; set; }

        public Decimal Ty_giaf { get; set; }

        public String Ghi_chu0 { get; set; }
        public String Hd_thue { get; set; }                // char(1)

        public String Tk_i { get; set; }
        public String Ma_kh_i { get; set; }

        public String Stt_rec_pc { get; set; }             // char(11) - liên kết phiếu chi
        public String So_ct_pc { get; set; }
        public String Ma_ct_pc { get; set; }
        public String Ma_qs_pc { get; set; }
        public String Ong_ba_pc { get; set; }
        public String Dien_giai_pc { get; set; }
        public String Ma_gd_pc { get; set; }



        public Int32 Tao_pc { get; set; }                   

        public String Id_hd { get; set; }                

        public String Ten_kh { get; set; }                  
        public String Ten_tk { get; set; }           
        public String Ma_so_thue { get; set; }



    }
}
