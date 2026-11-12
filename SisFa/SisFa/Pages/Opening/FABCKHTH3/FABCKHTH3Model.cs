using System;
using System.Collections.Generic;

namespace SisFa.Pages.Opening.FABCKHTH3
{
    [Serializable]
    public class FABCKHTH3Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Footer totals used by report designer
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public List<FABCKHTH3Row> LsData { get; set; }
        // Optional totals list for report compatibility (some reports expect a child collection "Tong")
        public List<lTC> Tong { get; set; }

        public FABCKHTH3Model()
        {
            LsData = new List<FABCKHTH3Row>();
            Tong = new List<lTC>();
        }
    }

    public class FABCKHTH3Row
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public string Ma_nhom { get; set; }
        public string Ten_nhom { get; set; }
        public string Ten_nhom2 { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_bpsd { get; set; }
        public string Loai_ts { get; set; }
        public string Nh_ts { get; set; }
        public string Tk_cp { get; set; }
        public string Tk_kh { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Nguyen_gia1 { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
    public class lTC
    {
        // multiple naming variants to support different report bindings
        public decimal t_gt_da_kh { get; set; }
        public decimal t_gt_kh_ky { get; set; }
        public decimal t_gt_da_kh1 { get; set; }

        // alternate names used in other reports / designer bindings
        public decimal gt_da_kh { get; set; }
        public decimal gt_kh_ky { get; set; }
        public decimal gt_da_kh1 { get; set; }

        // generic fields (for FABCGIAM-like reports)
        public decimal nguyen_gia { get; set; }
        public decimal gt_cl { get; set; }
    }
}
