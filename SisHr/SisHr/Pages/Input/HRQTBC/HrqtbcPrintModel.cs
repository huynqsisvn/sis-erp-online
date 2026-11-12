using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRQTBC
{
    [Serializable]
    public class HrqtbcPrintModel
    {
        public List<HrqtbcPrintRow> LsData { get; set; } = new List<HrqtbcPrintRow>();

        public HrqtbcPrintModel()
        {
            LsData = new List<HrqtbcPrintRow>();
        }
    }

    [Serializable]
    public class HrqtbcPrintRow
    {
        // Display fields - STT (order number)
        public int Stt { get; set; }

        // Database fields
        public string Ma_qtbc { get; set; }
        public string Ma_ns { get; set; }
        public string Nam_tot_nghiep { get; set; }
        public string Ma_cn { get; set; }
        public string Ma_qg { get; set; }
        public string Noi_cap { get; set; }
        public string Ma_tdbc { get; set; }
        public string Ma_htdt { get; set; }
        public string Xep_loai { get; set; }
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

        // Display fields - Major info
        public string Ten_cn { get; set; }
        public string Ten_cn2 { get; set; }

        // Display fields - Country info
        public string Ten_quocgia { get; set; }
        public string Ten_quocgia2 { get; set; }

        // Display fields - Certificate level info
        public string Ten_xlbc { get; set; }
        public string Ten_xlbc2 { get; set; }

        // Display fields - Education type info
        public string Ten_htdt { get; set; }
        public string Ten_htdt2 { get; set; }

        // Company info
        public string Tencty { get; set; }
        public string Diachicty { get; set; }
    }
}
