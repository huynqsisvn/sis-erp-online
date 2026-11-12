namespace SisGl.Pages.Reports.Glthuetk1TT80
{
    public class GlthueEditModel
    {
        public decimal ds { get; set; }
        public decimal thue { get; set; }

        public string dsText
        {
            get => ds.ToString("N0");
            set => ds = decimal.TryParse(value, out var v) ? v : 0;
        }

        public string thueText
        {
            get => thue.ToString("N0");
            set => thue = decimal.TryParse(value, out var v) ? v : 0;
        }

        public string chi_tieu { get; set; }
        public string ma_so { get; set; }
        public int stt { get; set; }
    }
}
