using System;
using System.Collections.Generic;

namespace SisCo.Pages.Reports.Copxcd2
{
    [Serializable]
    public class Copxcd2Model
    {
        public DateTime Ngay_ct1 { get; set; }
        public DateTime Ngay_ct2 { get; set; }
        public string Tk { get; set; }
        public string Ten_tk { get; set; }
        public string Ma_px { get; set; }

        public List<Copxcd2> LsData { get; set; }
        public List<Copxcd2Detail> LsDetail { get; set; }
        public List<Copxcd2Total> LsTotal { get; set; }

        public Copxcd2Model()
        {
            LsData = new List<Copxcd2>();
            LsDetail = new List<Copxcd2Detail>();
            LsTotal = new List<Copxcd2Total>();
        }
    }

    public class Copxcd2
    {
        public string Ma_px { get; set; }
        public string Ten_px { get; set; }
        public string Tk { get; set; }
        public decimal No_dk { get; set; }
        public decimal Co_dk { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal No_ck { get; set; }
        public decimal Co_ck { get; set; }
        public decimal No_dk_nt { get; set; }
        public decimal Co_dk_nt { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public decimal No_ck_nt { get; set; }
        public decimal Co_ck_nt { get; set; }
    }

    public class Copxcd2Detail
    {
        public DateTime Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public string Dien_giai { get; set; }
        public string Tk { get; set; }
        public string Tk_du { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public string Ma_nt { get; set; }
        public decimal Ty_gia { get; set; }
        public string Ma_px { get; set; }
        public string Ten_px { get; set; }
    }

    public class Copxcd2Total
    {
        public decimal Total_ps_no { get; set; }
        public decimal Total_ps_co { get; set; }
        public decimal Total_ps_no_nt { get; set; }
        public decimal Total_ps_co_nt { get; set; }
        public decimal Total_du_no_dau { get; set; }
        public decimal Total_du_co_dau { get; set; }
        public decimal Total_du_no_cuoi { get; set; }
        public decimal Total_du_co_cuoi { get; set; }
    }
}
