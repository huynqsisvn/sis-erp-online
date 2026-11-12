using SisSo.Model;

namespace SisSo.Pages.Input.Soycmh
{
    public class HYMModel
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph109> LSPh { get; set; }
        public List<Ct109> LSCt { get; set; }

    }
}
