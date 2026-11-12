using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGl.Pages.General.Glso2
{
    public class Glso2Model
    {
        public DateTime Tu_ngay { get; set; }
        public DateTime Den_ngay { get; set; }
        public int LoaiBC { get; set; }
        public int LoaiBoCT { get; set; }
        public List<Glso2> LsData { get; set; }
        public Glso2Model()
        {
            LsData = new List<Glso2>();
        }
    }
    public class Glso2
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
