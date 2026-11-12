using System;
using System.Collections.Generic;

namespace SisPo.Pages.Input.Poctpnb
{
    public class PNBModel
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }        
        public List<Ph72> LSPh { get; set; } = new();
        public List<Ct72> LSCt { get; set; } = new();
        public List<Ct72gt> LSCtgt { get; set; } = new();
        public string Ma_so_thue_cty { get; set; }       
        public int So_lien { get; set; } = 0;
        public string Ten_lien { get; set; }
        public string TotalString { get; set; }
        public string TotalTTString { get; set; }
        public string TotalStringNT { get; set; }
        public string TotalTTStringNT { get; set; }
        public string NgayString { get; set; }
        public string Ma_kho { get; set; }
        public string TknoString { get; set; }
        public decimal Thue_suat { get; set; }
    }
}

