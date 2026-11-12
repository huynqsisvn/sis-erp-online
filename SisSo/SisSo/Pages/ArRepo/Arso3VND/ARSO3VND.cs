namespace SisSo.Pages.ArRepo.Arso3VND
{
    [Serializable]
    public class ARSO3VNDModel
    {
        public DateTime Startdate { get; set; }     // Ngày bắt đầu từ giao diện lọc (ngay_ct)
        public DateTime Enddate { get; set; }
        public DateTime Ngay_ms { get; set; }
        public string LoaiBC { get; set; }        
        public string Bo_nkcd { get; set; }
        public string Ma_dvcs { get; set; }
        public List<ARSO3VND> LsDataCT { get; set; }
        public List<ARSO3VND> LsDataCT1 { get; set; }
        public List<ARSO3VND> LsData { get; set; }
        public decimal SoDuNoDauKy { get; set; }
        public decimal SoDuCoDauKy { get; set; }
        public decimal SoDuNoDauKyNt { get; set; }
        public decimal SoDuCoDauKyNt { get; set; }

        public decimal TongPsNo { get; set; }
        public decimal TongPsCo { get; set; }
        public decimal TongPsNoNt { get; set; }
        public decimal TongPsCoNt { get; set; }

        public decimal SoDuNoCuoiKy { get; set; }
        public decimal SoDuCoCuoiKy { get; set; }
        public decimal SoDuNoCuoiKyNt { get; set; }
        public decimal SoDuCoCuoiKyNt { get; set; }

        public string Tk { get; set; } = "";
        public string Ten_tk { get; set; } = "";
        public string Ten_tk2 { get; set; } = "";

        public string Ma_kh { get; set; } = "";
        public string Ma_nt { get; set; } = "";
        public string Ten_kh { get; set; } = "";
        public string Ten_kh2 { get; set; } = "";

        public ARSO3VNDModel()
        {
            LsData = new List<ARSO3VND>();
        }
    }

    public class ARSO3VND
    {
        public string Tk { get; set; }                   // Tài khoản
        public decimal No_dk { get; set; }               // Dư nợ đầu kỳ
        public decimal No_dk_nt { get; set; }            // Dư nợ đầu kỳ (NT)
        public decimal Co_dk { get; set; }               // Dư có đầu kỳ
        public decimal Co_dk_nt { get; set; }            // Dư có đầu kỳ (NT)
        public decimal Ps_no { get; set; }               // Phát sinh nợ
        public decimal Ps_no_nt { get; set; }            // Phát sinh nợ (NT)
        public decimal Ps_co { get; set; }               // Phát sinh có
        public decimal Ps_co_nt { get; set; }            // Phát sinh có (NT)
        public decimal So_du_no { get; set; }            // Số dư nợ (thay thế No_ck)
        public decimal So_du_no_nt { get; set; }         // Số dư nợ (NT) (thay thế No_ck_nt)
        public decimal So_du_co { get; set; }            // Số dư có (thay thế Co_ck)
        public decimal So_du_co_nt { get; set; }         // Số dư có (NT) (thay thế Co_ck_nt)
        public string Dia_chi { get; set; }
        public decimal So_ton { get; set; }
        public decimal So_ton_nt { get; set; }
        public string Dien_giai { get; set; }
        public string Dien_giai0 { get; set; }
        public string Dong { get; set; }
        public string Hd_pl { get; set; }
        public decimal Idu_co { get; set; }
        public decimal Idu_co_nt { get; set; }
        public decimal Idu_no { get; set; }
        public decimal Idu_no_nt { get; set; }
        public string Loai_hdong { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_ct { get; set; }
        public string Ma_ct_in { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ma_kh { get; set; }
        public string Ma_nt { get; set; }
        public string Ma_nvkd { get; set; }
        public string Ma_nguonkh { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_qs { get; set; }
        public string Ma_sp { get; set; }
        public string Ma_vv { get; set; }
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public string Nh_dk { get; set; }
        public int PageIndex { get; set; }
        public string So_ct { get; set; }
        public string So_lo { get; set; }
        public int Stt { get; set; }
        public decimal Stt_ct_nkc { get; set; }
        public string Stt_rec { get; set; }
        public string Stt_rec0 { get; set; }
        public string Ten_kh { get; set; }
        public string Ten_kh2 { get; set; }
        public string Ten_tk { get; set; }
        public string Ten_tk_du { get; set; }
        public string Ten_tk_du2 { get; set; }
        public string Ten_tk2 { get; set; }
        public string Tk_du { get; set; }
        public decimal Ty_gia { get; set; }
        public string Trang { get; set; }
    }
}
