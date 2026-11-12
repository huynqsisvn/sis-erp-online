using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCKHTH4
{
    [Serializable]
    public class FXBCKHTH4Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng cộng footer (dùng bởi report Designer: [t_nguyen_gia], [t_gt_kh_ky], [t_gt_da_kh], [t_gt_da_kh1], [t_gt_cl])
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FXBCKHTH4Row> LsData { get; set; }

        public FXBCKHTH4Model()
        {
            LsData = new List<FXBCKHTH4Row>();
        }
    }

    public class FXBCKHTH4Row
    {
        public int Stt { get; set; }
        // Fields for summary grouping by multiple criteria
        public string Loai_cc { get; set; }
        public string Ten_loai { get; set; }
        public string Ten_loai2 { get; set; }
        
        public string Ma_bpcc { get; set; }
        public string Ten_bpcc { get; set; }
        public string Ten_bpcc2 { get; set; }
        
        public string Nh_cc { get; set; }
        public string Ten_nh_cc { get; set; }
        public string Ten_nh_cc2 { get; set; }
        
        public string Tk_cp { get; set; }
        public string Ten_tk_cp { get; set; }
        public string Ten_tk_cp2 { get; set; }
        
        public string Nh_cc1 { get; set; }
        public string Ten_nh1 { get; set; }
        public string Ten_nh21 { get; set; }
        
        public string Nh_cc2 { get; set; }
        public string Ten_nh2 { get; set; }
        public string Ten_nh22 { get; set; }
        
        public string Nh_cc3 { get; set; }
        public string Ten_nh3 { get; set; }
        public string Ten_nh23 { get; set; }
        
        // Data fields
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl { get; set; }
        public int Bold { get; set; }
    }
}
