using System;
using System.Collections.Generic;

namespace SisHr.Pages.Input.HRBCCCDC
{
    [Serializable]
    public class HRBCCCDCModel
    {
        public DateTime Denngay { get; set; }
        public string Ma_dvcs { get; set; }

        public List<HRBCCCDCRow> LsData { get; set; }

        public HRBCCCDCModel()
        {
            LsData = new List<HRBCCCDCRow>();
        }
    }

    public class HRBCCCDCRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ten_ns_cp { get; set; }
        public string Ma_cc_me_cp { get; set; }
        public string Ten_cc_cp { get; set; }
        public decimal So_luong_cp { get; set; }
        public DateTime? Ngay_cap_cp { get; set; }
        public DateTime? Ngay_het_han_cp { get; set; }
        public string Ghi_chu_cp { get; set; }
        public string Ten_loai { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ma_cc_me_th { get; set; }
        public string Ten_cc_th { get; set; }
        public decimal So_luong_th { get; set; }
        public string Ma_ns_tu { get; set; }
        public string Ten_ns_tu { get; set; }
        public string Ma_ns_sang { get; set; }
        public string Ten_ns_sang { get; set; }
        public DateTime? Ngay_cap { get; set; }
        public string Ly_do_cap { get; set; }
        public DateTime? Ngay_dc { get; set; }
        public string Ly_do_dc { get; set; }
        public int Bold { get; set; }
    }
}
