using SisSo.Pages.Input.Soddh;

namespace SisSo.Pages.Input.Soddh
{
    public class DDHModel
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Dmhdm> LSPh { get; set; }
        public List<Dmhdmct> LSCt { get; set; }
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
