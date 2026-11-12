namespace SisGl.Pages.List.Glnhtk0
{
    public class LoaiTk
    {
        public string Ma_nh { get; set; }    // Mã loại (key)
        public string Ten_nh { get; set; }   // Tên loại
        public string Ten_nh2 { get; set; }
        public string Status { get; set; } = "1";

        // Audit
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int User_id { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int User_id0 { get; set; }
    }
}