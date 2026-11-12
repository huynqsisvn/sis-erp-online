using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FXBCTGTH1
{
    [Serializable]
    public class FXBCTGTH1Model
    {
        public int Nam { get; set; }
        public int TuKy { get; set; }
        public int DenKy { get; set; }
        public string v_title_report { get; set; }
        public string e_title_report { get; set; }
        public string value_detail_report { get; set; }
        public string v_tuKydenKy { get; set; }
        public string e_tuKydenKy { get; set; }

        public List<FXBCTGTH1Row> tbDetail { get; set; }

        public FXBCTGTH1Model()
        {
            tbDetail = new List<FXBCTGTH1Row>();
        }
    }

    [Serializable]
    public class FXBCTGTH1Row
    {
        public string khoa { get; set; }
        public string ma_tg_cc { get; set; }
        public string stt { get; set; }
        public string chi_tieu { get; set; }
        public string chi_tieu2 { get; set; }
        public int? bold { get; set; }
        public string ftag { get; set; }
        public string ftage { get; set; }
        public decimal? T_L_ { get; set; }

        public decimal? L_L01 { get; set; }
        public decimal? L_L02 { get; set; }
        public decimal? L_L03 { get; set; }
        public decimal? L_L04 { get; set; }
        public decimal? L_L05 { get; set; }
        public decimal? L_L06 { get; set; }
        public decimal? L_L07 { get; set; }
        public decimal? L_L08 { get; set; }
        public decimal? L_L09 { get; set; }
        public decimal? L_L10 { get; set; }
        public decimal? L_L11 { get; set; }
        public decimal? L_L12 { get; set; }
        public decimal? L_L13 { get; set; }
        public decimal? L_L14 { get; set; }
        public decimal? L_L15 { get; set; }
        public decimal? L_L16 { get; set; }
        public decimal? L_L17 { get; set; }
        public decimal? L_L18 { get; set; }
        public decimal? L_L19 { get; set; }
        public decimal? L_L20 { get; set; }
        public decimal? L_L21 { get; set; }
        public decimal? L_L22 { get; set; }
        public decimal? L_L23 { get; set; }
        public decimal? L_L24 { get; set; }
        public decimal? L_L25 { get; set; }
        public decimal? L_L26 { get; set; }
        public decimal? L_L27 { get; set; }
        public decimal? L_L28 { get; set; }
        public decimal? L_L29 { get; set; }
        public decimal? L_L30 { get; set; }
    }
}
