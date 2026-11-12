namespace SisGl.Pages.Input.GLCTPK1
{
    public class PK1Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph11> LSPh { get; set; }
        public List<Ct11> LSCt { get; set; }
        public List<Ct11gt> LSCtgt { get; set; }
        public string TotalStringVi { get; set; }
        public string TotalStringEn { get; set; }
        public string TotalStringVi_nt { get; set; }
        public string TotalStringEn_nt { get; set; }
    }
}
