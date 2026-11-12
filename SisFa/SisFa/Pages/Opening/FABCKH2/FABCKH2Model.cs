namespace SisFa.Pages.Opening.FABCKH2
{
    [Serializable]
    public class FABCKH2Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public List<FABCKH2Row> LsData { get; set; }

        public FABCKH2Model()
        {
            LsData = new List<FABCKH2Row>();
        }
    }

    public class FABCKH2Row
    {
        public int Stt { get; set; }
        public string Loai_ts { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_bpsd { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal So_ky_kh { get; set; }
        public decimal So_ngay_kh { get; set; }
        public decimal Gt_kh_ky1 { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Gt_kh_ky0 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal He_so { get; set; }
        public decimal Gt_cl { get; set; }
        public string Nh_ts { get; set; }
        public string Tk_ts { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
