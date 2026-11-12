using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.Poctpna
{
    public class Ct71
    {
        public int Stt { get; set; } 
        public String Stt_rec { get; set; }
        public String Stt_rec0 { get; set; }
        public String Ma_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public String So_ct { get; set; }

        public String Ma_kho_i { get; set; }  
        public String Ma_vv_i { get; set; } 
        public String Ma_vt { get; set; }            
        public String Tk_vt { get; set; }         

        public Decimal So_luong { get; set; }
        public String Dvt1 { get; set; }
        public Decimal He_so1 { get; set; }
        public Decimal So_luong1 { get; set; }

        // Giá & tiền SAU chiết khấu
        public Decimal Gia_nt { get; set; }
        public Decimal Gia { get; set; }
        public Decimal Tien_nt { get; set; }
        public Decimal Tien { get; set; }

        // Giá & tiền TRƯỚC chiết khấu (dùng khi có CK theo % hoặc tiền)
        public Decimal Gia_nt0 { get; set; }
        public Decimal Gia0 { get; set; }
        public Decimal Tien_nt0 { get; set; }
        public Decimal Tien0 { get; set; }

        // Chi phí / chiết khấu dòng
        public Decimal Cp_nt { get; set; }
        public Decimal Cp { get; set; }

        // Thuế GTGT dòng
        public Decimal Thue_nt { get; set; }
        public Decimal Thue { get; set; }

        public String Ma_td_i { get; set; }

        // Liên kết phiếu xuất kho (nếu tách riêng PX + HD)
        public String Stt_rec_px { get; set; }         // char(11)
        public String Stt_rec0px { get; set; }

        public String Ma_td2_i { get; set; }
        public String Ma_td3_i { get; set; }

        public DateTime? Ngay_td1_i { get; set; }
        public Decimal Sl_td1_i { get; set; }
        public Decimal Sl_td2_i { get; set; }
        public Decimal Sl_td3_i { get; set; }

        public String Gc_td1_i { get; set; }
        public String Gc_td2_i { get; set; }
        public String Gc_td3_i { get; set; }

        public String Tk_thue_i { get; set; }          // tài khoản thuế đầu ra chi tiết
        public String Ma_sp { get; set; }
        public Decimal Sl_sp { get; set; }

        public String Ma_phi_i { get; set; }
        public String So_lsx_i { get; set; }
        public String So_dh_i { get; set; }

        public String Lot_nbr { get; set; }
        public DateTime? Han_sd_lot { get; set; }

        public String Ma_nvhh { get; set; }
        public String Loc_id { get; set; }

        public String Dvt2 { get; set; }
        public Decimal So_luong2 { get; set; }

        public String Ma_bpht_i { get; set; }
        public String Ma_hd_i { get; set; }
        public String Ma_ku_i { get; set; }            // char(16)

        public DateTime? Ngay_s1_i { get; set; }
        public DateTime? Ngay_s2_i { get; set; }
        public String Gc_s1_i { get; set; }            // nvarchar(32)
        public String Gc_s2_i { get; set; }            // nvarchar(32)

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

        public Int32 Stt_dong { get; set; }              // số thứ tự dòng

        public Int32 Sua_tk_vt { get; set; }            // numeric(1,0) - cho phép sửa TK vật tư

        public String Ma_td4_i { get; set; }
        public String Ma_lo { get; set; }

        // ------------------- CHIẾT KHẤU DÒNG -------------------
        public Decimal Tl_ck { get; set; }             // tỷ lệ chiết khấu dòng (%)
        public Decimal Ck { get; set; }                // tiền chiết khấu VND
        public Decimal Ck_nt { get; set; }             // tiền chiết khấu ngoại tệ

        public String XT { get; set; }                 // char(13) - mã xuất khẩu / loại hàng đặc biệt

        public String Ghi_chu_tct { get; set; }        // ghi chú trên bảng kê (in hóa đơn)
        public String Ten_tk { get; set; }    
        public String Ten_kh { get; set; }
        public String Ten_vt { get; set; }
        public Decimal Ton13 { get; set; }
        public Decimal Vt_ton_kho { get; set; }
        public Decimal Gia_ton { get; set; }

        public String Ma_nx { get; set; }            

    }
}
