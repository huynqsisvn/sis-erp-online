namespace SisGl.Pages.General.Glsnkct
{
    public class GlsnkctModel
    {
        public DateTime Ngay_ct1 { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public string Condition { get; set; }
        public string MaDVCS { get; set; }
        public int Mau_bc { get; set; }
        public string ghi_co_tk { get; set; } // tiền tài khoản theo dõi (ghi có)
        public string ngay_ms { get; set; }
        public string col_header1 { get; set; }
        public string col_header2 { get; set; }
        public string col_header3 { get; set; }
        public string col_header4 { get; set; }
        // headers in foreign currency if needed
        public string col_header1_nt { get; set; }
        public string col_header2_nt { get; set; }
        public string col_header3_nt { get; set; }
        public string col_header4_nt { get; set; }

        // Summary list: reuse Glsnkt rows for detail lines
        public List<Glsnkct> LsData { get; set; } = new List<Glsnkct>();
    }

    // Reuse existing Glsnkt definition (kept minimal additions)
    public class Glsnkct
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
        public Decimal ColumnName1 { get; set; }
        public Decimal ColumnName1_nt { get; set; }
        public Decimal ColumnName2 { get; set; }
        public Decimal ColumnName2_nt { get; set; }
        public Decimal ColumnName3 { get; set; }
        public Decimal ColumnName3_nt { get; set; }
        public Decimal ColumnName4 { get; set; }
        public Decimal ColumnName4_nt { get; set; }

        // other/overflow and auxiliary
        public Decimal I_tien { get; set; }
        public Decimal I_tien_nt { get; set; }
        public Decimal I_ghi_no_co { get; set; } // Ghi nợ khác
        public Decimal I_ghi_no_co_nt { get; set; }
    }
}