using System;
using System.Collections.Generic;

namespace SisHr.Pages.Report.HRBCDSNSCTY
{
    [Serializable]
    public class HRBCDSNSCTYModel
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        public List<HRBCDSNSCTYRow> LsData { get; set; }

        public HRBCDSNSCTYModel()
        {
            LsData = new List<HRBCDSNSCTYRow>();
        }
    }

    public class HRBCDSNSCTYRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ma_cham_cong { get; set; }
        public string Ten_ns { get; set; }
        public string Ten_gioitinh { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public string Dia_chi { get; set; }
        public string Cmnd { get; set; }
        public DateTime? Ngay_cmnd { get; set; }
        public string Nc_cmnd { get; set; }
        public DateTime? Ngay_xn_ns { get; set; }
        public string Noi_xn_ns { get; set; }
        public string Dien_thoai { get; set; }
        public string Ten_vh { get; set; }
        public string Ten_bc { get; set; }
        public DateTime? Ngay_vao { get; set; }
        public DateTime? Ngay_hd { get; set; }
        public string Ten_lhdong { get; set; }
        public DateTime? Ngay_hl { get; set; }
        public string Ten_cv { get; set; }
        public string Ten_cd { get; set; }
        public string Bacluong { get; set; }
        public string So_bhxh { get; set; }
        public int Solanvipham { get; set; }
        public DateTime? Ngay_thoi_thuc_te { get; set; }
        public string Ghi_chu { get; set; }
        public string Ten_bpns { get; set; }
        public string Ma_bpns { get; set; }
        public string Ma_td_vanhoa { get; set; }
    }
}
