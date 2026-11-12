using DevExpress.Utils.Serializing.Helpers;
using SisGl.Pages.Input.GLCTPK1;
using System.Text.Json.Serialization;

namespace SisGl.Pages.Input.Glbtpb
{
    public class PK4Model
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }        
        public List<Inbtpb> LSCt { get; set; }
    }
    public class Inbtpb
    {
        public Int32 New_id { get; set; }
        public Decimal Id { get; set; }
        public Int64 Stt { get; set; }
        public Decimal Nam { get; set; }
        public Int32 Ky { get; set; }
        public String Stt_rec { get; set; }
        public String Stt_rec0 { get; set; }
        public String Nh_dk { get; set; }
        public String Nh_dkc { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public String So_ct { get; set; }
        public String Tk { get; set; }
        public String Tk_du { get; set; }
        public String Dien_giai { get; set; }
        public Decimal Ps_no { get; set; }
        public Decimal Ps_co { get; set; }
        public Decimal Ps_no_nt { get; set; }
        public Decimal Ps_co_nt { get; set; }
        public String Ma_vv { get; set; }
        public String Ma_bpht { get; set; }
        public String Ma_px { get; set; }
        public String Ma_phi { get; set; }
        public String Ma_sp { get; set; }
        public Int32 Fsort { get; set; }
    }
}
