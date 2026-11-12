using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCKHTH2
{
    [Serializable]
    public class FXBCKHTH2Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng cộng footer (dùng bởi report Designer: [t_gt_da_kh], [t_gt_kh_ky], [t_gt_da_kh1])
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public List<FXBCKHTH2Row> LsData { get; set; }

        public FXBCKHTH2Model()
        {
            LsData = new List<FXBCKHTH2Row>();
        }
    }

    public class FXBCKHTH2Row
    {
        public int Stt { get; set; }
        // Ma và Ten là field mà Detail band trong report dùng trực tiếp ([ma], [ten])
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ma_nhom { get; set; }
        public string Ten_nhom { get; set; }
        public string Ten_nhom2 { get; set; }
        public string So_the_cc { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_bpcc { get; set; }
        public string Loai_cc { get; set; }
        public string Nh_cc { get; set; }
        public string Tk_cp { get; set; }
        public string Tk_pb { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
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
}
