using System;
using System.Collections.Generic;

namespace SisFa.Pages.Input.Zcdmslts
{
    [Serializable]
    public class ZcdmsltsModel
    {
        public int    TuKy   { get; set; }
        public int    DenKy  { get; set; }
        public int    Nam    { get; set; }
        public string MaDvcs { get; set; } = "";

        // Tiêu ?? k? hi?n th? trên report (vd: "K?: 1-3 N?M 2025")
        public string KyNam  { get; set; } = "";

        public List<ZcdmsltsDetail> LsData { get; set; } = new List<ZcdmsltsDetail>();

        public ZcdmsltsModel()
        {
            LsData = new List<ZcdmsltsDetail>();
        }
    }

    [Serializable]
    public class ZcdmsltsDetail
    {
        public int     Stt       { get; set; }
        public string  So_the_ts { get; set; } = "";
        public string  Ten_ts    { get; set; } = "";
        public string  Ma_bpts   { get; set; } = "";
        public int     Nam       { get; set; }
        public int     Ky        { get; set; }
        public decimal Sl_kh     { get; set; }
    }
}
