using System;

namespace SisCo.Pages.Reports.Cothgtsppb
{
    [Serializable]
    public class CothgtsppbModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public int Ky2 { get; set; }
        public DateTime Ngay_ct1 { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public string Ma_px { get; set; }
        public string Ma_sp { get; set; }
        public string Tk { get; set; }

        public List<Cothgtsppb> LsData { get; set; }
        public List<CothgtsppbTotal> LsTotal { get; set; }

        public CothgtsppbModel()
        {
            LsData = new List<Cothgtsppb>();
            LsTotal = new List<CothgtsppbTotal>();
        }
    }

    public class Cothgtsppb
    {
        public Decimal Nam { get; set; }
        public Decimal Thang { get; set; }
        public String Stt { get; set; }
        public Decimal Tp { get; set; }
        public String Chi_tieu { get; set; }
        public String Chi_tieu2 { get; set; }
        public String Tk_du { get; set; }
        public String Tk { get; set; }
        public String Ma_px { get; set; }
        public String Ma_vt { get; set; }
        public Decimal Dd_dk { get; set; }
        public Decimal Dd_dk_nt { get; set; }
        public Decimal Ps_tk { get; set; }
        public Decimal Ps_tk_nt { get; set; }
        public Decimal Dd_ck { get; set; }
        public Decimal Dd_ck_nt { get; set; }
        public Decimal Gt_tk { get; set; }
        public Decimal Gt_tk_nt { get; set; }
        public Decimal So_luong { get; set; }
        public Decimal Gt_dv { get; set; }
        public Decimal Gt_dv_nt { get; set; }
        public Decimal He_so { get; set; }
        public Decimal He_soth { get; set; }
        public Decimal Thang2 { get; set; }
        public DateTime? Ngay_ct1 { get; set; }
        public DateTime? Ngay_ct2 { get; set; }
    }

    public class CothgtsppbTotal
    {
        public Decimal Dd_dk { get; set; }
        public Decimal Dd_dk_nt { get; set; }
        public Decimal Ps_tk { get; set; }
        public Decimal Ps_tk_nt { get; set; }
        public Decimal Dd_ck { get; set; }
        public Decimal Dd_ck_nt { get; set; }
        public Decimal Gt_tk { get; set; }
        public Decimal Gt_tk_nt { get; set; }
        public Decimal So_luong { get; set; }
        public Decimal Gt_dv { get; set; }
        public Decimal Gt_dv_nt { get; set; }
    }
}
