namespace SisSo.Pages.Reports.COLGH
{
    [Serializable]
    public class COLGHModel
    {
        public DateTime Startdate { get; set; } 
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }        
        public string Ma_dvcs { get; set; }

        public List<COLGH> LsData { get; set; }
        public List<dttk> LsDataCT2 { get; set; }
        public List<dtInfo> LsDataCT1 { get; set; }
        public COLGHModel()
        {
            LsData = new List<COLGH>();
        }
    }
    public class COLGH
    {
        public String Ma_kh { get; set; }
        public String Tk { get; set; }
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
        public int Stt { get; set; }               // stt
        public String Ten_kh { get; set; }         // Tên khách
        public String Ten_kh2 { get; set; }
        public String Nh_kh1 { get; set; }         // Nhóm khách 1
        public String Nh_kh2 { get; set; }         // Nhóm khách 2
        public String Nh_kh3 { get; set; }         // Nhóm khách 3

        public DateTime? Ngay_ct { get; set; }     // Ngày chứng từ
        public String So_ct { get; set; }          // Số chứng từ
        public String Dien_giai { get; set; }      // diễn giải
        public String Tk_du { get; set; }          // Tk đối ứng
        public String Ma_nt { get; set; }          // Mã ngoại tệ
        public decimal Ty_gia { get; set; }        // Tỷ giá
        public String Ma_vv { get; set; }          // Mã dự án / vụ việc
        public DateTime? Ngay_lct { get; set; }    // Ngày lập chứng từ
        public String Ma_qs { get; set; }          // Quyển số
        public String Ma_ct { get; set; }          // Mã chứng từ
        public String Ma_dvcs { get; set; }
    }
    public class dttk {
        public String Tk { get; set; }
        public String Ten_tk { get; set; }
        public String Ten_tk2 { get; set; }

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
