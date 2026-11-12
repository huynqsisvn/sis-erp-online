using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.Poctpna
{
    public class Ph71
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
        public String Ong_ba { get; set; }
        public String Dia_chi { get; set; }
        public String Ma_so_thue { get; set; }
        public String Dien_giai { get; set; }

        [Required]
        public String Ma_nx { get; set; }                  // tài khoản doanh thu

        public Decimal T_so_luong { get; set; }
        public Decimal T_soluong1 { get; set; }

        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }

        public Decimal T_tien_nt { get; set; }             // tổng tiền hàng ngoại tệ
        public Decimal T_tien { get; set; }                // tổng tiền hàng VND

        public Decimal T_tien_nt0 { get; set; }            // tiền hàng trước CK ngoại tệ
        public Decimal T_tien0 { get; set; }               // tiền hàng trước CK VND

        public Decimal T_cp_nt { get; set; }               // tổng chiết khấu, phụ phí NT
        public Decimal T_cp { get; set; }                  // tổng chiết khấu, phụ phí VND

        public Int32 Cp_thue_ck { get; set; }               // có chịu thuế CK không (0/1)

        public String Ma_thue { get; set; }
        public Decimal Thue_suat { get; set; }

        public Decimal T_thue_nt { get; set; }
        public Decimal T_thue { get; set; }

        public String Tk_thue_no { get; set; }             // TK thuế đầu ra
        public String Ma_kh2 { get; set; }

        public Int32 So_hd_gtgt { get; set; }              // numeric(2,0)
        public Decimal T_tt_nt { get; set; }               // tổng thanh toán NT
        public Decimal T_tt { get; set; }                  // tổng thanh toán VND

        public Int32 Han_tt { get; set; }                  // hạn thanh toán (ngày)
        public Int32 Thuetrului { get; set; }               // thuế trước lùi (đặc thù)

        public DateTime? Date { get; set; }
        public String Time { get; set; }
        public String User_id { get; set; }

        public DateTime? Date0 { get; set; }
        public String Time0 { get; set; }
        public String User_id0 { get; set; }

        public String Status { get; set; } 

        public String So_ct_tt { get; set; }

        public String So_lo1 { get; set; }
        public String Loai_hd { get; set; }
        public String Ma_bpht { get; set; }
        public String Loai_ct { get; set; }
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
        public Int32 Sua_tien { get; set; }

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
        [Required]
        public String Ma_qs { get; set; }
        public String Ma_thck { get; set; }

        public Decimal Ty_giaf { get; set; }

        public String Loai_pb { get; set; }                // char(1) - loại phiếu bổ sung
        public String T_ghi_chu { get; set; }              // nvarchar(max)

        public String Ma_kh_i { get; set; }
        public String Tk_i { get; set; }

        public String Hd_thue { get; set; }                // char(1)

        // Liên kết phiếu thu
        public String Stt_rec_pc { get; set; }             // char(11)
        public String So_ct_pc { get; set; }
        public String Ma_ct_pc { get; set; }
        public String Ma_qs_pc { get; set; }
        public String Ong_ba_pc { get; set; }
        public String Dien_giai_pc { get; set; }
        public String Ma_gd_pc { get; set; }


        public String Ten_kh { get; set; }

        public Int32 Tao_pc { get; set; }                   // tự động tạo phiếu thu

        public String Id_hd { get; set; }                  // ID hóa đơn điện tử (64 ký tự)

        public Decimal So_tien_pb { get; set; }            // số tiền phiếu bổ sung
        public Int32 Tag_pb { get; set; }                   // đánh dấu phiếu bổ sung

        // Chiết khấu
        public Decimal TL_CK { get; set; }                 // tỷ lệ chiết khấu (%)
        public Decimal TTNT2_T_CK { get; set; }            // tiền CK dòng chi tiết NT
        public Decimal TTNT0_T_CK { get; set; }            // tiền CK tổng NT

        public String XT { get; set; }                     // char(13) - mã xuất khẩu?

        public Decimal T_tien_sau_ck { get; set; }         // tổng tiền sau CK VND
        public Decimal T_tien_sau_ck_nt { get; set; }      // tổng tiền sau CK NT

        public Decimal T_ck { get; set; }                  // tổng tiền chiết khấu VND
        public Decimal T_ck_nt { get; set; }               // tổng tiền chiết khấu NT
        public String Nh_kh3 { get; set; }

        public String  Ten_tk { get; set; }               // tổng tiền chiết khấu NT

    }
}
