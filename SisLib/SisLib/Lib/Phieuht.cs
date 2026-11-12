using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisLib.Lib
{
    [Serializable]
    public class InPhieuhtModel
    {
        public DateTime? Ngay_ct { get; set; }
        public DateTime? Ngay_lct { get; set; }
        public String So_ct { get; set; }
        public String Ma_kh { get; set; }
        public String Ten_kh { get; set; }
        public String Dien_giai { get; set; }
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }                
        public List<Phieuct> LSCt { get; set; }
        public string TotalString { get; set; }
        public string DateString { get; set; }
        public string Tk_no { get; set; }
        public string Tk_co { get; set; }
        public int So_lien { get; set; } = 0;
        public string Ten_lien { get; set; }
    }
    [Serializable]
    public class Phieuct
    {
        public Int32 New_id { get; set; }
        public Int64 Stt { get; set; }
        public Decimal Id { get; set; }
        public String Stt_rec { get; set; }
        public String Stt_rec0 { get; set; }
        public String Ma_ct { get; set; }
        public String Ma_nk { get; set; }
        public String Ma_gd { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public DateTime? Ngay_lct { get; set; }
        public String So_ct { get; set; }
        public String So_dh { get; set; }
        public String So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public String Ong_ba { get; set; }
        public String Dien_giai { get; set; }
        public String Nh_dk { get; set; }
        public String Tk { get; set; }
        public String Tk_du { get; set; }
        public Decimal Ps_no_nt { get; set; }
        public Decimal Ps_co_nt { get; set; }
        public String Ma_nt { get; set; }
        public Decimal Ty_gia { get; set; }
        public Decimal Ty_gia_ht { get; set; }
        public Decimal Ty_gia_ht2 { get; set; }
        public Decimal Ps_no { get; set; }
        public Decimal Ps_co { get; set; }
        public String Ma_kh { get; set; }
        public String Ma_bp { get; set; }
        public Decimal Ct_nxt { get; set; }
        public String Ma_vv { get; set; }
        public String Status { get; set; }
        public String Ma_dvcs { get; set; }
        public String Ma_td { get; set; }
        public String Ma_ku { get; set; }
        public String So_ct0 { get; set; }
        public String So_seri0 { get; set; }
        public DateTime? Ngay_ct0 { get; set; }
        public DateTime? Date { get; set; }
        public String Time { get; set; }
        public Int32 User_id { get; set; }
        public DateTime? Date0 { get; set; }
        public String Time0 { get; set; }
        public Int32 User_id0 { get; set; }
        public String Ma_td2 { get; set; }
        public String Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public String Ma_sp { get; set; }
        public Decimal Sl_sp { get; set; }
        public String So_lsx { get; set; }
        public String Ma_bpht { get; set; }
        public String Ma_phi { get; set; }
        public String Loai_ct { get; set; }
        public String Ma_hd { get; set; }
        public String Gc0 { get; set; }
        public String Dien_giai0 { get; set; }
        public String Ma_ns { get; set; }
        public String Ma_tb { get; set; }
        public String Ma_hdm { get; set; }
        public String So_ct_tc { get; set; }
        public DateTime? Ngay_ct_tc { get; set; }
        public String Ma_px { get; set; }
        public String Ma_qs { get; set; }
        public Decimal Sua_tggs { get; set; }
        public Decimal Ty_giaf { get; set; }
        public Decimal Ty_gia_htf { get; set; }
        public Decimal Ty_giahtf2 { get; set; }
        public String Ma_thck { get; set; }
        public String Stt_ctgs { get; set; }
        public String Nh_dkc { get; set; }
        public String Ma_td4 { get; set; }
        public Decimal Sl_td1 { get; set; }
        public Decimal Sl_td2 { get; set; }
        public Decimal Sl_td3 { get; set; }
        public String Gc_td1 { get; set; }
        public String Gc_td2 { get; set; }
        public String Gc_td3 { get; set; }
        public Decimal Tl_ck { get; set; }
        public String Mau_hddt { get; set; }
        public String So_seri_hddt { get; set; }
        public String So_ct_hddt { get; set; }
        public String Ma_hdcr { get; set; }
        public String Ten_tk { get; set; }
        public String Ten_tk2 { get; set; }
        public String Ten_kh { get; set; }
        public String Ten_kh2 { get; set; }
        public String Ten_kh_in { get; set; }
        public String Ten_kh_in2 { get; set; }
    }
}
