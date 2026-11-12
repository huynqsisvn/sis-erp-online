using SisSo.Pages.Input.Sodmhdb;

namespace SisSo.Pages.Input.Sobgncc
{
    public class BGNCCModel
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph111> LSPh { get; set; }
        public List<Ct111> LSCt { get; set; }
        public string TotalString { get; set; }
        public string T_tien_nt_String { get; set; }
        public string T_tien_nt2_String { get; set; }
        public string DateString { get; set; }
        public string Tk_no { get; set; }
        public string Tk_co { get; set; }
        public int So_lien { get; set; } = 0;
        public string Ten_lien { get; set; }
        public List<HachToan> LstHachToan { get; set; }
        public decimal T_ps_no { get; set; }
        public decimal T_ps_co { get; set; }
        public MST MST { get; set; }
        public MST MST_kh { get; set; }
        public string Ten_cty { get; set; }
        public string Dia_chi_cty { get; set; }
        public string Ma_so_thue_cty { get; set; }
        public string So_tk_nh_cty { get; set; }
        public string Dien_thoai_cty { get; set; }
     
        public string TotalTTString { get; set; }
        public string TotalStringNT { get; set; }
        public string TotalTTStringNT { get; set; }
        public string TotalStringE { get; set; }
        public string TotalTTStringE { get; set; }
        public string TotalStringNTE { get; set; }
        public string TotalTTStringNTE { get; set; }
        public string TknoString { get; set; }
        public string TkcoString { get; set; }
        public string NgayString { get; set; }
        public string Ma_kho { get; set; }
        public decimal Thue_suat { get; set; }
    }
    public class HachToan
    {
        public string Tk { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public string Dien_giai { get; set; }
        public int Stt { get; set; }
    }

    public class MST
    {
        public string So_1 { get; set; }
        public string So_2 { get; set; }
        public string So_3 { get; set; }
        public string So_4 { get; set; }
        public string So_5 { get; set; }
        public string So_6 { get; set; }
        public string So_7 { get; set; }
        public string So_8 { get; set; }
        public string So_9 { get; set; }
        public string So_10 { get; set; }
        public string So_11 { get; set; }
        public string So_12 { get; set; }
        public string So_13 { get; set; }
        public string So_14 { get; set; }
    }
}
