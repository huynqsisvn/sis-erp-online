using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCa.Model
{
    public class Ct41
    {
        public int Stt { get; set; }
        public string Stt_rec { get; set; }
        public string Stt_rec_tt { get; set; }
        public string Stt_rec0 { get; set; }
        public string Ma_ct { get; set; }
        public string So_ct { get; set; }
        public string Ma_nt0 { get; set; }
        public string So_ct0 { get; set; }
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_ct0 { get; set; }
        public string Dien_giaii { get; set; }
        public string Ma_kh_i { get; set; }
        [Required]
        public string Tk_i { get; set; }
        public string Ten_tk_i { get; set; }
        public string Ten_tk_i2 { get; set; }
        public string Ma_nt_i { get; set; }
        public string Ma_vv_i { get; set; }
        public string So_dh_i { get; set; }
        public string Ma_bpht_i { get; set; }
        public string Ma_phi_i { get; set; }
        public string Ten_phi_i { get; set; }
        public decimal Ty_gia { get; set; }
        public decimal Tien { get; set; }
        public decimal Tien_nt { get; set; }
        public decimal Tien_tt { get; set; }
        public decimal T_tt_nt0 { get; set; }
        public decimal Ty_giahtf2 { get; set; }
        public decimal Ty_gia_ht2 { get; set; }
        public decimal T_tt_qd { get; set; }
        public decimal Phai_tt_nt { get; set; }
        public decimal Phai_tt_nt2 { get; set; }
        public decimal Tt_qd { get; set; }    
        public decimal Tien_cltg { get; set; }   
    }
}
