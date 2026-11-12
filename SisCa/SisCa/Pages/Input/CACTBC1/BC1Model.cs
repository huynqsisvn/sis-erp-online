namespace SisCa.Pages.Input.CACTBC1
{
    public class BC1Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph51> LSPh { get; set; }
        public List<Ct51> LSCt { get; set; }
        public TkPs Ps { get; set; }
        public List<Ps> LSTkPs { get; set; }
        public string TotalStringVi { get; set; }
        public string TotalStringEn { get; set; }
        public string TotalStringVi_nt { get; set; }
        public string TotalStringEn_nt { get; set; }
        public string TotalString { get; set; }
        public string DateString { get; set; }
        public string Ten_kh { get; set; }
        public string Ma_nt0 { get; set; }
    }
    public class TkPs
    {
        public string Tk_co { get; set; }
        public string Ps_no { get; set; }
        public string Ps_co { get; set; }
        public string Ps_no_nt { get; set; }
        public string Ps_co_nt { get; set; }
    }
    public class Ps
    {
        public string Tk_co { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public int Stt { get; set; }
        public string Dien_giai { get; set; }
        public string T_ps { get; set; }

    }
}
