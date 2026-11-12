using SisGl;
using SisGl.Pages;
using SisGl.Pages.Voucherb;
using SisGl.Pages.Voucherb.Glgsso1;
using SisGl.Pages.Voucherb.Glgsso4;
using System;
using System.Collections.Generic;

namespace SisGl.Pages.Voucherb.Glgsso4
{
    [Serializable]
    public class Glgsso4Model
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
        public List<Glgsso4> LsData { get; set; }
        public List<Glgsso4Summary> LsSummary { get; set; }

        public Glgsso4Model()
        {
            LsData = new List<Glgsso4>();
            LsSummary = new List<Glgsso4Summary>();
        }
    }

    public class Glgsso4
    {
        public int Stt { get; set; }
        public string Stt_ctgs { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string So_ct { get; set; }
        public string So_lo { get; set; }
        public DateTime? Ngay_lo { get; set; }
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

    public class Glgsso4Summary
    {
        public decimal No_dk { get; set; }
        public decimal Co_dk { get; set; }
        public decimal No_dk_nt { get; set; }
        public decimal Co_dk_nt { get; set; }
        public decimal Ps_no { get; set; }
        public decimal Ps_co { get; set; }
        public decimal Ps_no_nt { get; set; }
        public decimal Ps_co_nt { get; set; }
        public decimal No_ck { get; set; }
        public decimal Co_ck { get; set; }
        public decimal No_ck_nt { get; set; }
        public decimal Co_ck_nt { get; set; }
    }
}
