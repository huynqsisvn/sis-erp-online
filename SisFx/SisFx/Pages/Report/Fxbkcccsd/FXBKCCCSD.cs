using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.Fxbkcccsd
{
    [Serializable]
    public class FXBKCCCSDModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Dau_cuoi { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng cộng footer (dùng bởi report Designer)
        public decimal T_nguyen_gia { get; set; }
        public List<FXBKCCCSDRow> LsData { get; set; }

        public FXBKCCCSDModel()
        {
            LsData = new List<FXBKCCCSDRow>();
        }
    }

    public class FXBKCCCSDRow
    {
        public int Stt { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ma_cc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public decimal Nguyen_gia { get; set; }
        public DateTime Ngay_mua { get; set; }
        public int So_ky_kh { get; set; }
        public DateTime Ngay_kh0 { get; set; }
        public DateTime Ngay_kh1 { get; set; }
        public string Ma_bpcc { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Ma_dvcs { get; set; }
        public decimal Sl_cc { get; set; }
        public int Ftag { get; set; }
        public int Bold { get; set; }
    }
}
