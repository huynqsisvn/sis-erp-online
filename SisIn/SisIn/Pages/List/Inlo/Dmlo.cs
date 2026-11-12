namespace SisIn.Pages.List.Inlo
{
    public class Dmlo
    {
        public string Ma_lo { get; set; } = string.Empty;         // Batch code
        public string Ma_tra_cuu { get; set; } = string.Empty;    // Lookup code
        public string Ten_lo { get; set; } = string.Empty;        // Batch name
        public string Ten_lo2 { get; set; } = string.Empty;       // Batch name 2
        public DateTime? Ngay_sx { get; set; }                    // Production date
        public DateTime? Ngay_hh { get; set; }                    // Expiry date
        public string Status { get; set; } = "1";                 // Status (0/1)
        public string Ghi_chu { get; set; } = string.Empty;       // Note

        // Audit fields
        public DateTime? Date { get; set; }                       // Last modified date
        public string Time { get; set; } = string.Empty;          // Last modified time
        public int? User_id { get; set; }                         // Last modified user
        public string User_name { get; set; } = string.Empty;     // Last modified user name
        public DateTime? Date0 { get; set; }                      // Created date
        public string Time0 { get; set; } = string.Empty;         // Created time
        public int? User_id0 { get; set; }                        // Created user
        public string User_name0 { get; set; } = string.Empty;    // Created user name
    }
}
