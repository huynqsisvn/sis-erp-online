namespace SisFa.Pages.Opening.FABCPBKH
{
    [Serializable]
    public class FABCPBKHModel
    {
        public int Nam { get; set; }
        public int Ky1 { get; set; }
        public int Ky2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }
        public List<FABCPBKHRow> LsData { get; set; }

        public FABCPBKHModel()
        {
            LsData = new List<FABCPBKHRow>();
        }
    }

    public class FABCPBKHRow
    {
        public int Stt { get; set; }
        public int Nam { get; set; }
        public string Loai_ts { get; set; }
        public string So_the_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ma_bpsd { get; set; }
        public string Tk_cp { get; set; }
        public string Tk_kh { get; set; }
        public string Ma_phi { get; set; }
        public string Ma_bpht { get; set; }
        public string Ma_du_an { get; set; }
        public string Ma_sp { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Nguyen_gia1 { get; set; }
        public decimal Gt_kh_ky { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_da_kh1 { get; set; }
        public decimal Gt_cl { get; set; }
        public decimal Gt_cl1 { get; set; }
        public string Nh_ts { get; set; }
        public string Nh_ts1 { get; set; }
        public string Nh_ts2 { get; set; }
        public string Nh_ts3 { get; set; }
        public int Bold { get; set; }
        public string Ftag { get; set; }
    }
}
