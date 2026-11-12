using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRCPCCDC
{
    [Serializable]
    public class HrcpccdcModel
    {
        public string So_ct { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string Dien_giai { get; set; } = "";
        public string DateString { get; set; } = "";
        public List<HrcpccdcDtPrint> LsDt { get; set; } = new List<HrcpccdcDtPrint>();

        public HrcpccdcModel()
        {
            LsDt = new List<HrcpccdcDtPrint>();
        }
    }

    [Serializable]
    public class HrcpccdcDtPrint
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; } = "";
        public string Ten_ns { get; set; } = "";
        public string Ma_bpns { get; set; } = "";
        public string Ma_chucvu { get; set; } = "";
        public string Ma_cc_me { get; set; } = "";
        public string Ten_cc { get; set; } = "";
        public decimal So_luong { get; set; }
        public DateTime? Ngay_cap { get; set; }
        public DateTime? Ngay_het_han { get; set; }
    }
}
