using SisCa.Pages.Input.CACTPC1;

namespace SisCa.Pages.Input.CACTBN1
{
    public class BN1Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph56> LSPh { get; set; }
        public List<Ct56> LSCt { get; set; }
        public TkPs LSTkPs { get; set; }
        public PrintNH LSNH { get; set; }
        public string Ten_cty { get; set; }
        public string TotalStringVi { get; set; }
        public string TotalStringEn { get; set; }
        public string TotalStringVi_nt { get; set; }
        public string TotalStringEn_nt { get; set; }
        public string TotalString { get; set; }
        public string DateString { get; set; }
        public string Ma_nt0 { get; set; }
        public string Dia_chi { get; set; }
        public string Ma_so_thue { get; set; }

    }
    public class TkPs
    {
        public string Tk_no { get; set; }
        public string Ps_no { get; set; }
        public string Ps_co { get; set; }
        public string Ps_no_nt { get; set; }
        public string Ps_co_nt { get; set; }
    }
    public class PrintNH
    {
        public string Ten_nh { get; set; }
        public string Tk_nh { get; set; }
        public string Ten{ get; set; }
        public string Dia_chi{ get; set; }
        public string Stk { get; set; }
        public string Tai_nh { get; set; }
        public string Tinh_thanh { get; set; }
        public string Noi_dung { get; set; }
        public string Dien_giai { get; set; }
        public int So_ct_goc { get; set; }
        public int So_lien { get; set; }
    }   
}
