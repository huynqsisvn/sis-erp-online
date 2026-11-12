namespace SisFa.Pages.Opening.FABCKHBP2
{
    [Serializable]
    public class FABCKHBP2Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // Tổng cộng footer (nếu cần)
        public decimal T_nguyen_gia { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_cl { get; set; }
        public List<FABCKHBP2Row> LsData { get; set; }

        public FABCKHBP2Model()
        {
            LsData = new List<FABCKHBP2Row>();
        }
    }

    public class FABCKHBP2Row
    {
        public int Stt { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_bpsd { get; set; }
        public string Loai_ts { get; set; }
        public string Nh_ts { get; set; }
        public string Tk_cp { get; set; }
        public string Tk_kh { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Nguyen_gia1 { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public DateTime? Ngay_kh0 { get; set; }
        public decimal So_ky_kh { get; set; }
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
