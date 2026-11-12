namespace SisGl.Pages.General.GLSNKBH
{
    public class GLSNKBHModel
    {
        public DateTime Ngay_ct1 { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public string Condition { get; set; }
        public string MaDVCS { get; set; }
        public int Mau_bc { get; set; }
        public string ghi_co_tk { get; set; } // tiền tài khoản theo dõi (ghi có)
        public string ngay_ms { get; set; }

        public List<GLSNKBH> LsData { get; set; } = new List<GLSNKBH>();
    }

    // Reuse existing GLSNKBH definition (kept minimal additions)
    public class GLSNKBH
    {
        public String Stt_rec { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_ct_in { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public String So_ct { get; set; }
        public String So_ct1 { get; set; }
        public DateTime? Ngay { get; set; }
        public String Tag { get; set; }
        public Int32 Stt { get; set; }
        public Int32 Stt1 { get; set; }
        public DateTime? Ngay_lct { get; set; }
        public String Ma_qs { get; set; }
        public String Ma_dvcs { get; set; }
        public String Ma_kh { get; set; }
        public String Ten_kh { get; set; }
        public String Ten_kh2 { get; set; }
        public String Dien_giai { get; set; }
        public String Tk { get; set; }      // debit account
        public String Tk_du { get; set; }   // credit account
        public Decimal Tien { get; set; }
        public Decimal Tien_nt { get; set; }
        public Decimal Tc { get; set; }
        public Decimal Tc_nt { get; set; }
        public Decimal Ghi_no_co { get; set; }
        public Decimal Ghi_no_co_nt { get; set; }

        // columns for mapped debit accounts (up to 4)
        public Decimal Column1 { get; set; }
        public Decimal Column2 { get; set; }
        public Decimal Column3 { get; set; }
        public Decimal I_tien { get; set; }
        public Decimal I_tien_nt { get; set; }
        public Decimal I_ghi_no_co { get; set; } // Ghi nợ khác
        public Decimal I_ghi_no_co_nt { get; set; }
    }
}