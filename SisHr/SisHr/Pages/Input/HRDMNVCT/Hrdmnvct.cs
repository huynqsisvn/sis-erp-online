using System;

namespace SisHr.Pages.Input.HRDMNVCT
{
    public class Hrdmnvct
    {
        public int? Nvctid { get; set; }
        public string Ma_ns { get; set; }
        public string Ma_quyet_dinh { get; set; }
        public DateTime? Ngay_di { get; set; }
        public DateTime? Ngay_ve { get; set; }
        public string Ma_nct { get; set; }
        public int? Chi_phi { get; set; }
        public string Noi_dung { get; set; }
        public string Ket_qua { get; set; }
        public string Ghi_chu { get; set; }
        public int? Status { get; set; }

        // Audit fields
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }

        // Display-only fields from view
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        public string Ten_nct { get; set; }
        public string User_name { get; set; }
        public string User_name0 { get; set; }
    }
}
