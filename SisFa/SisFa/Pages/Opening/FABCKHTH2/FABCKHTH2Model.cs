namespace SisFa.Pages.Opening.FABCKHTH2
{
    [Serializable]
    public class FABCKHTH2Model
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        // T?ng c?ng footer (dùng b?i report Designer: [t_gt_da_kh], [t_gt_kh_ky], [t_gt_da_kh1])
        public decimal T_gt_da_kh { get; set; }
        public decimal T_gt_kh_ky { get; set; }
        public decimal T_gt_da_kh1 { get; set; }
        public List<FABCKHTH2Row> LsData { get; set; }

        public FABCKHTH2Model()
        {
            LsData = new List<FABCKHTH2Row>();
        }
    }

    public class FABCKHTH2Row
    {
        public int Stt { get; set; }
        // Ma và Ten là field mà Detail band trong report dùng tr?c ti?p ([ma], [ten])
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string Ma_nhom { get; set; }
        public string Ten_nhom { get; set; }
        public string Ten_nhom2 { get; set; }
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
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
