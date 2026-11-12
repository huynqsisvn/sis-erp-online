using SisCa.Pages.Input.APCTPN2;

namespace SisCa.Pages.Input.APCTPN2
{
    public class PN2Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph32> LSPh { get; set; }
        public List<Ct32> LSCt { get; set; }
        public List<Ct32_Detail> LSCtDetails { get; set; }
        public TkPs LSTkPs { get; set; }
        public string TotalString { get; set; }
        public string DateString { get; set; }
        public string TotalStringVi { get; set; }
        public string TotalStringEn { get; set; }
        public string TotalStringVi_nt { get; set; }
        public string TotalStringEn_nt { get; set; }
        public string Ma_nt0 { get; set; }

    }
    public class TkPs
    {
        public string Tk_no { get; set; }
        public string Ps_no { get; set; }   
        public string Ps_no_nt { get; set; }
    }
}
