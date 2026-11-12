using System;
using System.Collections.Generic;

namespace SisFa.Pages.Opening.FABCGIAM
{
    [Serializable]
    public class FABCGIAMModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string MauBC { get; set; }

        // Totals
        

        public List<FABCGIAMRow> LsData { get; set; }
        public List<lTC> Tong { get; set; }

        public FABCGIAMModel()
        {
            LsData = new List<FABCGIAMRow>();
            Tong = new List<lTC>();
        }
    }

    public class FABCGIAMRow
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public DateTime? Ngay_tg { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public string Ten_tg_ts { get; set; }
        public string Dien_giai { get; set; }

        // additional fields used in grid
        public string Nh_ts { get; set; }
        public string Loai_ts { get; set; }
        public string Ma_bpts { get; set; }
        public string Tk_cp { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_nv { get; set; }
        public string So_the_ts { get; set; }
    }
    public class lTC
    {
        public decimal nguyen_gia { get; set; }
        public decimal gt_da_kh { get; set; }
        public decimal gt_cl { get; set; }
    }
}
