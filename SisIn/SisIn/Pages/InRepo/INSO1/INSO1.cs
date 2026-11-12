namespace SisIn.Pages.InRepo.INSO1
{
    [Serializable]
    public class INSO1Model
    {
        public DateTime Startdate { get; set; } 
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }        
        public string Ma_dvcs { get; set; }
        public string Ma_kho { get; set; }
        public int? To_so { get; set; }
        public string Ma_vt { get; set; }
        public string Tk_vt { get; set; }
        public string Ten_vt { get; set; }
        public string Ten_vt2 { get; set; }
        public string Ten_Kho { get; set; }
        public string Ten_Kho2 { get; set; }
        public DateTime? NgayMs { get; set; }
        public string Dvt { get; set; }
        public List<INSO1> LsData { get; set; }
        public List<INSO1Detail> LsDetail { get; set; }
        public INSO1Model()
        {
            LsData = new List<INSO1>();
        }
    }
    public class INSO1
    {
        public int Stt { get; set; }
        public string Ma_vt { get; set; }
        public string Ten_vt { get; set; }
        public string Ten_vt2 { get; set; }
        public string Dvt { get; set; }
        public DateTime NgayMs { get; set; }
        public decimal Ton_dau { get; set; }
        public decimal Du_dau { get; set; }
        public decimal Du_dau_nt { get; set; }

        public decimal Sl_nhap { get; set; }
        public decimal Tien_nhap { get; set; }
        public decimal Tien_nt_n { get; set; }

        public decimal Sl_xuat { get; set; }
        public decimal Tien_xuat { get; set; }
        public decimal Tien_nt_x { get; set; }

        public decimal Ton_cuoi { get; set; }
        public decimal Du_cuoi { get; set; }
        public decimal Du_cuoi_nt { get; set; }

        public string Tk_vt { get; set; }
        public string Nh_vt1 { get; set; }
        public string Nh_vt2 { get; set; }
        public string Nh_vt3 { get; set; }
       
        public DateTime Ngay_ct { get; set; }
        public DateTime Ngay_lct { get; set; }
        public string So_ct_nhap { get; set; }
        public string So_ct_xuat { get; set; }
        public string So_ct { get; set; }
        public string Ma_ct { get; set; }
        public string Ma_qs { get; set; }
        public string Ma_gd { get; set; }

        public string Ma_kh { get; set; }
        public string Ten_kh { get; set; }
        public string Ten_kh2 { get; set; }
        public string Ma_kho { get; set; }
        public string Ma_nx { get; set; }

        public string Ma_nt { get; set; }
        public decimal Ty_gia { get; set; }
        public decimal Gia { get; set; }
        public decimal Gia_nt { get; set; }

        public decimal Sl_ton { get; set; }
        public decimal Ton { get; set; }
        public decimal Ton_nt { get; set; }

        public string Dien_giai { get; set; }
        public decimal Pn_gia_tb { get; set; }
        public decimal Px_gia_dd { get; set; }
        public string Ma_dvcs { get; set; }
    }
   
    public class INSO1Detail
    {
        public string Ma_vt { get; set; }

        public decimal Tondk { get; set; }

        public decimal Dudk { get; set; }
        public decimal Du_ntdk { get; set; }

        public decimal Tonck { get; set; }
        public decimal Duck { get; set; }
        public decimal Du_ntck { get; set; }
    }
}
