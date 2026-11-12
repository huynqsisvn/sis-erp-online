namespace SisGl.Pages.Reports.Glthuetk1TT80
{
    [Serializable]
    public class Glthuetk1TT80Model
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }
        public string Ma_dvcs { get; set; }

        public List<Gthuebk2tt119> LsData { get; set; }
        public List<dtInfo> LsSoDu { get; set; }
        public List<Gthuebk2tt119> LsDetail { get; set; }
        public decimal Ps_no_trong_ky { get; set; } = 0m;
        public decimal Ps_co_trong_ky { get; set; } = 0m;
        public decimal Ps_no_trong_ky_nt { get; set; } = 0m;
        public decimal Ps_co_trong_ky_nt { get; set; } = 0m;
        public string Ma_vt { get; set; } = "";
        public string Ten_vt { get; set; } = "";
        public string Ten_vt2 { get; set; } = "";

        public string Ma_kh { get; set; } = "";
        public string Ten_kh { get; set; } = "";
        public string Ten_kh2 { get; set; } = "";
        public Glthuetk1TT80Model()
        {
            LsData = new List<Gthuebk2tt119>();
        }
    }
    public class EditTaxModel
    {
        public string ChiTieu { get; set; }
        public decimal Ds { get; set; }
        public decimal DsNt { get; set; }
        public decimal Thue { get; set; }
        public decimal ThueNt { get; set; }

        // key để update
        public int Stt { get; set; }
        public string MaSo { get; set; }
    }

    public class Gthuebk2tt119
    {
        public String Ma_kh { get; set; }
        public String Tk { get; set; }
        public String Tk_du { get; set; }
        public String Tk0 { get; set; }
        public string Ma_vt { get; set; } = "";
        public string Ten_vt { get; set; } = "";
        public string Ten_vt2 { get; set; } = "";
        public string Ten_kh { get; set; } = "";
        public string Ten_kh2 { get; set; } = "";
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
        public Decimal Du_no { get; set; }
        public Decimal Du_no_nt { get; set; }
        public Decimal Du_co { get; set; }
        public Decimal Du_co_nt { get; set; }
        public Decimal No_dn { get; set; }
        public Decimal No_dn_nt { get; set; }
        public Decimal Co_dn { get; set; }
        public Decimal Co_dn_nt { get; set; }

        public string Dvt { get; set; }
        public decimal Gia { get; set; }
        public decimal Tien0 { get; set; }
        public decimal Cp { get; set; }
        public decimal Gia2 { get; set; }
        public string Tk_vt_dmvt { get; set; }
        public string Nh_vt1 { get; set; }
        public string Nh_vt2 { get; set; }
        public string Nh_vt3 { get; set; }

        public DateTime Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public string Ma_kho { get; set; }
        public decimal Gia_nt { get; set; }
        public decimal Tien_nt_n { get; set; }
        public string Ma_nt { get; set; }
        public decimal Ty_giaf { get; set; }
        public decimal Nk { get; set; }
        public decimal Db { get; set; }
        public decimal Gia_gt { get; set; }
        public string Ma_vv { get; set; }
        public string Ma_hdm { get; set; }
        public string Ma_hd { get; set; }
        public string Dien_giai { get; set; }
        public string Tk_vt { get; set; }
        public DateTime Ngay_lct { get; set; }
        public string Ma_qs { get; set; }
        public string Ma_ct { get; set; }

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
        public int Stt { get; set; }
        public string Nh_kh1 { get; set; }  // Nhóm NCC 1
        public string Đvt { get; set; }
        public Decimal Sl_nhap { get; set; }
        public Decimal Tien_nhap { get; set; }
        public string Nh_kh2 { get; set; }  // Nhóm NCC 2
        public string Nh_kh3 { get; set; }  // Nhóm NCC 3
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
        public int Stt { get; set; }        // Số thứ tự
        public string Ten_kh { get; set; }  // Tên nhà cung cấp
        public string Nh_kh1 { get; set; }  // Nhóm NCC 1
        public string Nh_kh2 { get; set; }  // Nhóm NCC 2
        public string Nh_kh3 { get; set; }  // Nhóm NCC 3

    }
}

