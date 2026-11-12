using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Pages.Mskt
{
    public class Dmmskt
    {
        public Guid? Inv_id { get; set; } = Guid.NewGuid();
        public string Ma_kh { get; set; }
        public string Ma_nhsp { get; set; }
        public string Ma_sp { get; set; }        
        public string? Ghi_chu { get; set; }        
        public string? User_name0 { get; set; }
        public int User_id0 { get; set; }
        public string Time0 { get; set; }        
        public DateTime Date0 { get; set; }        
        public string Ma_tra_cuu { get; set; }
        public string Ms_ket_qua { get; set; }
    }
}
