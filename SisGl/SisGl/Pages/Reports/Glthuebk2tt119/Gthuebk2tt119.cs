namespace SisGl.Pages.Reports.Glthuebk2tt119
{
    [Serializable]
    public class Glthuebk2tt119Model
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }
        public string Ma_dvcs { get; set; }

        public List<Glthuebk3tt119> LsData { get; set; }
        public Glthuebk2tt119Model()
        {
            LsData = new List<Glthuebk3tt119>();
        }
    }

    public class Glthuebk3tt119
    {
        public int Stt { get; set; }

        public string Ma_qs { get; set; }

        public string So_ct0 { get; set; }

        public DateTime? Ngay_ct0 { get; set; }

        public string Ma_kh { get; set; }

        public string Ten_kh { get; set; }

        public string Dia_chi { get; set; }

        public string Ma_so_thue { get; set; }

        public string Ten_vt { get; set; }

        public decimal T_tien2 { get; set; }

        public string Ma_thue { get; set; }

        public decimal Thue_suat { get; set; }

        public decimal T_thue { get; set; }

        public decimal T_tt { get; set; }

        public string Ghi_chu { get; set; }

        public string Tk_thue_co { get; set; }

        public string Tk_du { get; set; }

        public string Ma_vv { get; set; }

        public string Ma_hd { get; set; }
        public string Ma_dvcs { get; set; }

        public string Ma_ct { get; set; }

        public DateTime? Ngay_ct { get; set; }
        public string So_seri { get; set; }
    }


}

