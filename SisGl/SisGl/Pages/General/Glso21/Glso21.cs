using System;
using System.Collections.Generic;

namespace SisGl.Pages.General.Glso21
{
    [Serializable]
    public class Glso21Model
    {
        public DateTime Startdate { get; set; }     // Ngày bắt đầu từ giao diện lọc (ngay_ct)
        public DateTime Enddate { get; set; }     
        public string LoaiBC { get; set; }        
        public string Bo_nkcd { get; set; }          
        public string Ma_dvcs { get; set; }          

        public List<Glso21> LsData { get; set; }     // Danh sách chi tiết hóa đơn

        public Glso21Model()
        {
            LsData = new List<Glso21>();
        }
    }

    public class Glso21
    {
        public Int32 Stt { get; set; }
        public DateTime Ngay_ct { get; set; }
        public String So_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public String Ma_kh { get; set; }
        public String Ten_kh { get; set; }
        public String Dien_giai { get; set; }
        public String Tk { get; set; }
        public Decimal Ps_no { get; set; }
        public Decimal Ps_co { get; set; }
        public String Ma_vv { get; set; }
        public String Ten_tk { get; set; }
        public String Ma_qs { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_dvcs { get; set; }
        public Decimal Stt_ct_nkc { get; set; }
        public String Stt_rec { get; set; }
        public String Nh_dkc { get; set; }
        public String Stt_rec0 { get; set; }
        public String FSort { get; set; }
        public Int32 Sort_ps_no { get; set; }
        public String So_ct_soft { get; set; }
        public Int32 Tag { get; set; }
    }


}