namespace SisGl.Pages.General.Glth1a
{
    [Serializable]
    public class Glth1aModel
    {
        public DateTime Startdate { get; set; } 
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }        
        public string Ma_dvcs { get; set; }

        public List<Glth1a> LsData { get; set; }
        public List<dtInfo> LsSoDu { get; set; }
        public List<Glth1a> LsDetail { get; set; }
        public decimal Ps_no_trong_ky { get; set; } = 0m;
        public decimal Ps_co_trong_ky { get; set; } = 0m;
        public decimal Ps_no_trong_ky_nt { get; set; } = 0m;
        public decimal Ps_co_trong_ky_nt { get; set; } = 0m;
        public Glth1aModel()
        {
            LsData = new List<Glth1a>();
        }
    }
    public class Glth1a
    {
        public String Ma_kh { get; set; }
        public String Tk { get; set; }
        public String Tk_du { get; set; }
        public String Tk0 { get; set; }
        public Decimal Loai_tk { get; set; }
        public Decimal Tk_cn { get; set; }
        public Decimal Tk_sc { get; set; }
        public Decimal Bac_tk { get; set; }
        public String Ten_tk { get; set; }
        public String Ten_tk2 { get; set; }
        public Decimal Bold { get; set; }
        public String Ma_cdkt { get; set; }
        public String Ts_nv { get; set; }
        public String Khong_am { get; set; }
        public Decimal No_dk { get; set; }
        public Decimal No_dk_nt { get; set; }
        public Decimal Co_dk { get; set; }
        public Decimal Co_dk_nt { get; set; }
        public Decimal Ps_no { get; set; }
        public Decimal Ps_no_nt { get; set; }
        public Decimal Ps_co { get; set; }
        public Decimal Ps_co_nt { get; set; }
        public Decimal No_dn { get; set; }
        public Decimal No_dn_nt { get; set; }
        public Decimal Co_dn { get; set; }
        public Decimal Co_dn_nt { get; set; }
        public Decimal Ps_no1 { get; set; }
        public Decimal Ps_no_nt1 { get; set; }
        public Decimal Ps_co1 { get; set; }
        public Decimal Ps_co_nt1 { get; set; }
        public Decimal Lk_no { get; set; }
        public Decimal Lk_no_nt { get; set; }
        public Decimal Lk_co { get; set; }
        public Decimal Lk_co_nt { get; set; }
        public Decimal No_ck { get; set; }
        public Decimal No_ck_nt { get; set; }
        public Decimal Co_ck { get; set; }
        public Decimal Co_ck_nt { get; set; }
    }
    public class dtInfo
    {
        public Decimal No_dk { get; set; }
        public Decimal No_dk_nt { get; set; }
        public Decimal Co_dk { get; set; }
        public Decimal Co_dk_nt { get; set; }
        public Decimal Ps_no { get; set; }
        public Decimal Ps_no_nt { get; set; }
        public Decimal Ps_co { get; set; }
        public Decimal Ps_co_nt { get; set; }
        public Decimal No_dn { get; set; }
        public Decimal No_dn_nt { get; set; }
        public Decimal Co_dn { get; set; }
        public Decimal Co_dn_nt { get; set; }
        public Decimal Ps_no1 { get; set; }
        public Decimal Ps_no_nt1 { get; set; }
        public Decimal Ps_co1 { get; set; }
        public Decimal Ps_co_nt1 { get; set; }
        public Decimal Lk_no { get; set; }
        public Decimal Lk_no_nt { get; set; }
        public Decimal Lk_co { get; set; }
        public Decimal Lk_co_nt { get; set; }
        public Decimal No_ck { get; set; }
        public Decimal No_ck_nt { get; set; }
        public Decimal Co_ck { get; set; }
        public Decimal Co_ck_nt { get; set; }
    }
}
