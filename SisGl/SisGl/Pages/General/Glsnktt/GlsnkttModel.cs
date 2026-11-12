namespace SisGl.Pages.General.Glsnktt
{
    public class GlsnkttModel
    {
        public DateTime Ngay_ct1 { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public string Condition { get; set; }
        public string MaDVCS { get; set; }
        public int Mau_bc { get; set; }
        public string ghi_co_tk { get; set; }
        public string ngay_ms { get; set; }
        public string col_header1 { get; set; }
        public string col_header2 { get; set; }
        public string col_header3 { get; set; }
        public string col_header4 { get; set; }
        public string col_header1_nt { get; set; }
        public string col_header2_nt { get; set; }
        public string col_header3_nt { get; set; }
        public string col_header4_nt { get; set; }

        public List<Glsnktt> LsData { get; set; } = new List<Glsnktt>();
    }
    public class Glsnktt
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
        public String Tk { get; set; }
        public String Tk_du { get; set; }
        public Decimal Tien { get; set; }
        public Decimal Tien_nt { get; set; }
        public Decimal Tc { get; set; }
        public Decimal Tc_nt { get; set; }
        public Decimal Ghi_no_co { get; set; }
        public Decimal Ghi_no_co_nt { get; set; }
        public Decimal ColumnName1 { get; set; }
        public Decimal ColumnName1_nt { get; set; }
        public Decimal ColumnName2 { get; set; }
        public Decimal ColumnName2_nt { get; set; }
        public Decimal ColumnName3 { get; set; }
        public Decimal ColumnName3_nt { get; set; }
        public Decimal ColumnName4 { get; set; }
        public Decimal ColumnName4_nt { get; set; }
        public Decimal I_columnName1 { get; set; }
        public Decimal I_columnName1_nt { get; set; }
        public Decimal I_columnName2 { get; set; }
        public Decimal I_columnName2_nt { get; set; }
        public Decimal I_columnName3 { get; set; }
        public Decimal I_columnName3_nt { get; set; }
        public Decimal I_columnName4 { get; set; }
        public Decimal I_columnName4_nt { get; set; }
        public Decimal I_tien { get; set; }
        public Decimal I_tien_nt { get; set; }
        public Decimal I_ghi_no_co { get; set; }
        public Decimal I_ghi_no_co_nt { get; set; }
    }
}