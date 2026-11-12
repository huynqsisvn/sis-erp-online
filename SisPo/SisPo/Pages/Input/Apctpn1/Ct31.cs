using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.Apctpn1
{
    public class Ct31
    {
        public int Stt { get; set; }                      
        public String Stt_rec { get; set; }
        public String Stt_rec0 { get; set; }
        public String Ma_ct { get; set; }
        public DateTime Ngay_ct { get; set; }
        public String So_ct { get; set; }

        public String Tk_vt { get; set; }                      // tài khoản vật tư
        public String Dien_giaii { get; set; }                 // diễn giải dòng (diễn giải chi tiết)

        public String Dvt { get; set; }                        // đơn vị tính chính
        public Decimal So_luong { get; set; }

        public Decimal Gia_nt { get; set; }                    // đơn giá ngoại tệ
        public Decimal Gia { get; set; }                       // đơn giá VND
        public Decimal Tien_nt { get; set; }                   // thành tiền ngoại tệ
        public Decimal Tien { get; set; }                      // thành tiền VND

        public Decimal Thue_nt { get; set; }                   // tiền thuế ngoại tệ
        public Decimal Thue { get; set; }                      // tiền thuế VND

        public String Ma_vv_i { get; set; }                    // mã vụ việc
        public String Ma_td_i { get; set; }
        public String Ma_td2_i { get; set; }
        public String Ma_td3_i { get; set; }

        public DateTime? Ngay_td1_i { get; set; }
        public Decimal Sl_td1_i { get; set; }
        public Decimal Sl_td2_i { get; set; }
        public Decimal Sl_td3_i { get; set; }

        public String Gc_td1_i { get; set; }
        public String Gc_td2_i { get; set; }
        public String Gc_td3_i { get; set; }

        public String Tk_thue_i { get; set; }                  // tài khoản thuế đầu vào chi tiết
        public String Ma_sp { get; set; }
        public Decimal Sl_sp { get; set; }

        public Decimal Cp { get; set; }                        // chi phí phân bổ
        public Decimal Cp_nt { get; set; }

        public String Ma_phi_i { get; set; }
        public String So_lsx_i { get; set; }
        public String So_dh_i { get; set; }

        public String Ma_bpht_i { get; set; }
        public String Ma_hd_i { get; set; }
        public String Ma_ku_i { get; set; }                    // char(16)

        public DateTime? Ngay_s1_i { get; set; }
        public DateTime? Ngay_s2_i { get; set; }
        public String Gc_s1_i { get; set; }                    // nvarchar(32)
        public String Gc_s2_i { get; set; }                    // nvarchar(32)

        public Decimal Sl_s1_i { get; set; }
        public Decimal Sl_s2_i { get; set; }

        public String Ma_s1_i { get; set; }
        public String Ma_s2_i { get; set; }

        public String Gc0_i { get; set; }
        public String Ma_ns_i { get; set; }
        public String Ma_tb_i { get; set; }
        public String Ma_hdm_i { get; set; }

        public String So_ct_tc { get; set; }                   // dùng udt_ten_dien_giai → có thể dài hơn
        public DateTime? Ngay_ct_tc { get; set; }

        public String Ma_nvhh { get; set; }
        public String Lot_nbr { get; set; }
        public DateTime? Han_sd_lot { get; set; }

        public String Loc_id { get; set; }

        public String Dvt2 { get; set; }                       // đơn vị tính phụ
        public Decimal So_luong2 { get; set; }

        public String Ma_px_i { get; set; }

        public Int32 Stt_dong { get; set; }                      // số thứ tự dòng (udt_stt_dong)

        public String Ma_td4_i { get; set; }
        public String Ten_kh { get; set; }
        public String Ten_tk { get; set; }


    }
}
