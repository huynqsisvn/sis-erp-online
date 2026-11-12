using System;
using System.Collections.Generic;

namespace SisPo.Pages.Input.Apctpn1
{
    [Serializable]
    public class PN1Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph31> LSPh { get; set; } = new();
        public List<Ct31> LSCt { get; set; } = new();
        public List<Ct31> LSCtgt { get; set; } = new();

        // Hỗ trợ in ấn
        public string Ten_cty { get; set; }
        public string Dia_chi_cty { get; set; }
        public string Ma_so_thue_cty { get; set; }
        public string So_tk_nh_cty { get; set; }
        public string Dien_thoai_cty { get; set; }
        public int So_lien { get; set; } = 0;
        public string Ten_lien { get; set; }
        public string TotalString { get; set; }
        public string TotalTTString { get; set; }
        public string TknoString { get; set; }
        public string TkcoString { get; set; }
        public string NgayString { get; set; }
        public string Ma_kho { get; set; }
        public decimal Thue_suat { get; set; }
        public List<HachToan> LSTkPs { get; set; }
        public decimal T_ps_no { get; set; }
        public decimal T_ps_co { get; set; }


    }
    public class HachToan
    {
        public string Tk { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public string Dien_giai { get; set; }
        public int Stt { get; set; }
    }

}
