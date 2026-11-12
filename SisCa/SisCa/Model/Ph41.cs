using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCa.Model
{
    public class Ph41
    {
        public string Tk { get; set; }
        public string Ten_nx { get; set; }
        public string Ten_nx2 { get; set; }
        public string Stt_rec { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_ct { get; set; }
        public string So_ct { get; set; }
        public string Ma_qs { get; set; }
        public string Ma_gd { get; set; }
        public string Ma_bpht { get; set; }
        public string Ten_bpht { get; set; }
        public string Ten_bpht2 { get; set; }
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public string Ma_kh { get; set; }
        public string Ten_kh { get; set; }
        public string Ten_kh2 { get; set; }
        public string Ong_ba { get; set; }
        public string Dia_chi { get; set; }
        public string Dien_giai { get; set; }     
        public string Ma_nt { get; set; }
        public decimal Ty_gia { get; set; }
        public decimal Ty_giaf { get; set; }
        public decimal T_tien { get; set; }
        public decimal T_tien_nt { get; set; }
        public decimal T_Tien_tt { get; set; }
        public decimal T_Tien_cltg { get; set; }
        public string Status { get; set; }
        public int User_id { get; set; }
        public int User_id0 { get; set; }
        public string Time { get; set; }
        public string Time0 { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date0 { get; set; }
        public string? Files_dinhkem { get; set; }  
    }
}
