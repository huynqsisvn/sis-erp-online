using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRQTCT
{
    [Serializable]
    public class HrqtctPrintModel
    {
        public List<HrqtctPrintRow> LsData { get; set; } = new List<HrqtctPrintRow>();

        public HrqtctPrintModel()
        {
            LsData = new List<HrqtctPrintRow>();
        }
    }

    [Serializable]
    public class HrqtctPrintRow
    {
        // Database fields
        public string Ma_qtct { get; set; }
        public string Ma_ns { get; set; }
        public int Stt { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
        public string Cong_viec { get; set; }
        public string Donvi_ct { get; set; }
        public string Ma_cv { get; set; }
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

        // Display fields - Employee info
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        
        // Display fields - Position info
        public string Ten_cv { get; set; }
        public string Ten_cv2 { get; set; }
        
        // Company info
        public string Tencty { get; set; }
        public string Diachicty { get; set; }

        // Formatted dates
        public string Tu_ngay_Formatted
        {
            get { return Tu_ngay.HasValue ? Tu_ngay.Value.ToString("dd/MM/yyyy") : ""; }
        }

        public string Den_ngay_Formatted
        {
            get { return Den_ngay.HasValue ? Den_ngay.Value.ToString("dd/MM/yyyy") : ""; }
        }
    }
}
