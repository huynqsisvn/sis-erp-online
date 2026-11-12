using System;

namespace SisHr.Pages.Input.HRXLNS
{
    public class HrxlnsRow
    {
        public string Idxl { get; set; }
        public int Nam { get; set; }
        public int Thang { get; set; }
        public string Ma_ns { get; set; }
        public string Ma_xlld { get; set; }
        public string Ghi_chu { get; set; }
        public string Status { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display fields (from view)
        public string Ten_ns { get; set; }
        public string Ten_xlld { get; set; }
    }
}
