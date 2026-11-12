using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.DMDXCP
{
    [Serializable]
    public class DmdxcpModel
    {
        public string So_ct { get; set; } = "";
        public string Ma_ns { get; set; } = "";
        public string Ten_ns { get; set; } = "";
        public string Ten_bpns { get; set; } = "";
        public DateTime? Ngay_ct { get; set; }
        public string DateString { get; set; } = "";
        public List<DmdxcpDtPrint> LsDt { get; set; } = new List<DmdxcpDtPrint>();

        public DmdxcpModel()
        {
            LsDt = new List<DmdxcpDtPrint>();
        }
    }

    [Serializable]
    public class DmdxcpDtPrint
    {
        public int Stt { get; set; }
        public string Noi_dung { get; set; } = "";
        public string Dvt { get; set; } = "";
        public decimal So_luong { get; set; }
        public decimal Don_gia { get; set; }
        public decimal Thanh_tien { get; set; }
        public string Ghi_chu { get; set; } = "";
    }
}
