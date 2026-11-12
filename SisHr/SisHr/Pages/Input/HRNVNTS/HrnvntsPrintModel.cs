using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRNVNTS
{
    public class HrnvntsPrintRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public string Ten_bpns { get; set; }
        public DateTime? Ngay_nghi { get; set; }
        public DateTime? Ngay_di_lam_lai { get; set; }
        public string Ghi_chu { get; set; }
    }

    public class HrnvntsPrintModel
    {
        public List<HrnvntsPrintRow> LsData { get; set; } = new List<HrnvntsPrintRow>();
    }
}
