using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRDCCCDC
{
    [Serializable]
    public class HrdccccdcModel
    {
        public string So_ct { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string Dien_giai { get; set; } = "";
        public string Ten_loai { get; set; } = "";
        public string DateString { get; set; } = "";
        public List<HrdccccdcDtPrint> LsDt { get; set; } = new List<HrdccccdcDtPrint>();

        public HrdccccdcModel()
        {
            LsDt = new List<HrdccccdcDtPrint>();
        }
    }

    [Serializable]
    public class HrdccccdcDtPrint
    {
        public int Stt { get; set; }
        public string Ma_ns_tu { get; set; } = "";
        public string Ten_ns_tu { get; set; } = "";
        public string Ma_ns_sang { get; set; } = "";
        public string Ten_ns_sang { get; set; } = "";
        public string Ma_bpns { get; set; } = "";
        public string Ma_chucvu { get; set; } = "";
        public string Ma_cc_me { get; set; } = "";
        public string Ten_cc { get; set; } = "";
        public decimal So_luong { get; set; }
        public DateTime? Ngay_cap { get; set; }
        public string Ly_do_cap { get; set; } = "";
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_dc { get; set; } = "";
    }
}
