namespace SisIn.Pages.List.Dmbpht
{
    public class Dmbpht
    {
        public String Ma_bpht { get; set; }
        public String Ten_bpht { get; set; }
        public String Ten_bpht2 { get; set; }
        public String Ma_tra_cuu { get; set; }
        public String Ma_dvcs { get; set; }
        public String Ten_dvcs { get; set; }
        public String Ten_dvcs2 { get; set; }
        public String Tk_dd { get; set; }
        public String Ten_tk_dd { get; set; }
        public String Ten_tk_dd2 { get; set; }
        public String Status { get; set; }
        public String Status2 { get; set; }
        
        // Audit fields
        public DateTime? Date { get; set; }
        public Int32 User_id { get; set; }
        public String User_name { get; set; }
        public DateTime? Date0 { get; set; }
        public Int32 User_id0 { get; set; }
        public String User_name0 { get; set; }
        public String Time { get; set; }
        public String Time0 { get; set; }
        public String Search { get; set; }
    }
}