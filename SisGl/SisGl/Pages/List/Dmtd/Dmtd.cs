namespace SisGl.Pages.List.Dmtd
{
    public class Dmtd
    {
        public string ma_td { get; set; }          // Mã t? do
        public string ten_td { get; set; }         // Tên t? do
        public string ten_td2 { get; set; }        // Tên t? do 2
        public string ma_tra_cuu { get; set; }     // Mã tra c?u
        public string ma_dvcs { get; set; }        // Mã ??n v? c? s?
        public int status { get; set; }            // Tr?ng thái s? d?ng
        public string ghi_chu { get; set; }        // Ghi chú

        // Audit fields
        public DateTime? Date { get; set; }        // Ngày s?a
        public string Time { get; set; }           // Gi? s?a
        public string User_id { get; set; }        // ID ng??i s?a
        public string User_name { get; set; }      // Tên ng??i s?a
        public DateTime? Date0 { get; set; }       // Ngày t?o
        public string Time0 { get; set; }          // Gi? t?o
        public string User_id0 { get; set; }       // ID ng??i t?o
        public string User_name0 { get; set; }     // Tên ng??i t?o
    }
}
