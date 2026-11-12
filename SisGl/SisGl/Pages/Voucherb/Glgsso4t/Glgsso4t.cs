using SisGl.Pages.Voucherb.Glgsso4;
using System;
using System.Collections.Generic;

namespace SisGl.Pages.Voucherb.Glgsso4t
{
    public class Glgsso4tModel
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime Ngay_ms { get; set; }
        public string Tk { get; set; }
        public string Ten_tk { get; set; }
        public string Ten_tk2 { get; set; }
        public string Ma_dvcs { get; set; }

        public decimal So_du_dau_vnd { get; set; }
        public decimal So_du_dau_nt { get; set; }
        public decimal So_du_cuoi_vnd { get; set; }
        public decimal So_du_cuoi_nt { get; set; }
        public string Ten_so_du_dau_vi_vnd { get; set; }
        public string Ten_so_du_dau_vi_nt { get; set; }
        public string Ten_so_du_dau_en_vnd { get; set; }
        public string Ten_so_du_dau_en_nt { get; set; }
        public string Ten_so_du_cuoi_vi_vnd { get; set; }
        public string Ten_so_du_cuoi_vi_nt { get; set; }
        public string Ten_so_du_cuoi_en_vnd { get; set; }
        public string Ten_so_du_cuoi_en_nt { get; set; }
        public List<Glgsso4t> LsData { get; set; }
        public List<Glgsso4tSummary> LsSummary { get; set; }

        public Glgsso4tModel()
        {
            LsData = new List<Glgsso4t>();
            LsSummary = new List<Glgsso4tSummary>();
        }
    }

    public class Glgsso4t
    {
        public int Stt { get; set; }
        public string Stt_ctgs { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_lo { get; set; }
        public string So_lo { get; set; }
        public string Dien_giai { get; set; }
        public string Tk { get; set; }
        public string Tk_du { get; set; }
        public string Ten_tk_du { get; set; }
        public string Ten_tk_du2 { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_co_nt { get; set; }
        public decimal Idu_no { get; set; }
        public decimal Idu_no_nt { get; set; }
        public decimal Idu_co { get; set; }
        public decimal Idu_co_nt { get; set; }
        public string Ma_nt { get; set; }
        public decimal Ty_gia { get; set; }
        public string Ma_kh { get; set; }
        public string Ten_kh { get; set; }
        public string Ten_kh2 { get; set; }
        public string Ma_vv { get; set; }
        public string Ma_qs { get; set; }
        public string Ma_ct { get; set; }
        public string Ma_dvcs { get; set; }
    }

    public class Glgsso4tSummary
    {
        public decimal Du_no { get; set; }
        public decimal Du_co { get; set; }
        public decimal Du_no_nt { get; set; }
        public decimal Du_co_nt { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public decimal Du_ck_no { get; set; }
        public decimal Du_ck_co { get; set; }
        public decimal Du_ck_no_nt { get; set; }
        public decimal Du_ck_co_nt { get; set; }
    }
}
