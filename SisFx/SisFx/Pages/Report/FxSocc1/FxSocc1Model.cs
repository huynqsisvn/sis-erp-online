using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FxSocc1
{
    [Serializable]
    public class FxSocc1Model
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public DateTime? Ngay_giam_ts { get; set; }
        public DateTime? Ngay_ms { get; set; }
        public string Ma_bpcc { get; set; }
        public string Ma_dvcs { get; set; }

        public List<FxSocc1Row> LsData { get; set; }

        public FxSocc1Model()
        {
            LsData = new List<FxSocc1Row>();
        }
    }

    public class FxSocc1Row
    {
        public int Id { get; set; }
        public int? Stt { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_cc { get; set; }
        public string Nuoc_sx { get; set; }
        public DateTime? Ngay_mua { get; set; }
        public string So_hieu_cc { get; set; }
        public decimal Nguyen_gia { get; set; }
        public int So_ky_kh { get; set; }
        public string Ly_do_giam { get; set; }
        public string Ma_giam_cc { get; set; }
        public string Ten_giam { get; set; }
        public string Ten_giam2 { get; set; }
        public decimal Tl_kh { get; set; }
        public decimal Muc_kh { get; set; }
        public decimal Gt_da_kh { get; set; }
        public string So_ct_giam { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public string Dvt { get; set; }
        public decimal So_luong { get; set; }
        public decimal Tien { get; set; }
        public decimal Sl_giam { get; set; }
        public decimal Tien_giam { get; set; }
        public string Ghi_chu { get; set; }
        public string Nh_cc { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_bpcc { get; set; }
        public string Tk_cc { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Loai_cc { get; set; }
    }
}
