using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisIn.Pages.Input.Inctpnd
{
    public class PNDModel
    {
        public string Tieu_de1 { get; set; }
        public string Tieu_de2 { get; set; }
        public byte[] Print_logo { get; set; }
        public List<Ph74> LSPh { get; set; }
        public List<Ct74> LSCt { get; set; }
        public string TotalString { get; set; }
        public string DateString { get; set; }
        public string TotalStringVi { get; set; }
        public string TotalStringEn { get; set; }
        public string TotalStringVi_nt { get; set; }
        public string TotalStringEn_nt { get; set; }
        public string Tk_no { get; set; }
        public string Tk_co { get; set; }
        public int So_lien { get; set; } = 0;
        public string Ten_lien { get; set; }
    }
}
