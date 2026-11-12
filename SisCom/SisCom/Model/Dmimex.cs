using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Model
{
    public class Dmimex
    {
        public string Ma_imex { get; set; }
        public string Loai { get; set; }
        public string Ten { get; set; }
        public string Ten2 { get; set; }
        public string Dbf_mau { get; set; }
        public string Ex_mau { get; set; }
        public string Dbf_im { get; set; }
        public string Khoa { get; set; }
        public string Kt_trung { get; set; }
        public string Overwrite { get; set; }
        public string Kt_long { get; set; }
        public string Bang_ma { get; set; }
        public string Ma_qs { get; set; }
        public string Postproc { get; set; }
        public string Ph { get; set; }
        public string Ct { get; set; }
        public string Ma_ct { get; set; }
        public string Ma_phan_he { get; set; }
        public string Vbrowse1 { get; set; }
        public string Ebrowse1 { get; set; }
    }
    public class DataErrArgs
    {
        public DataSet DataErr { get; set; }
        public string Ma_imex { get; set; }

        public DataErrArgs(DataSet data, string maimex)
        {
            DataErr = data;
            Ma_imex = maimex;
        }
    }
}
