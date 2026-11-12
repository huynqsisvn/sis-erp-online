using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.DMXNNS
{
    [Serializable]
    public class DmxnnsModel
    {
        public string Dia_chi { get; set; } = "";
        public string Ten_ns_giao { get; set; } = "";
        public string Ma_bpns_giao { get; set; } = "";
        public string Ten_ns_nhan { get; set; } = "";
        public string Ma_bpns_nhan { get; set; } = "";
        public string Noi_dung { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string DateString { get; set; } = "";
        public List<DmxnnsDtPrint> LsDt { get; set; } = new List<DmxnnsDtPrint>();

        public DmxnnsModel()
        {
            LsDt = new List<DmxnnsDtPrint>();
        }
    }

    [Serializable]
    public class DmxnnsDtPrint
    {
        public int Stt { get; set; }
        public string Ten_giayto { get; set; } = "";
        public bool Ban_chinh { get; set; }
        public bool Ban_sao { get; set; }
        public int So_luong_banchinh { get; set; }  // 1 n?u Ban_chinh = true, 0 n?u false
        public int So_luong_bansao { get; set; }    // 1 n?u Ban_sao = true, 0 n?u false
        public decimal So_luong { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ghi_chu { get; set; } = "";
    }
}
