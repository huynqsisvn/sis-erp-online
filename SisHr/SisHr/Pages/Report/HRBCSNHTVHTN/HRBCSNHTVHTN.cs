using System;
using System.Collections.Generic;

namespace SisHr.Pages.Report.HRBCSNHTVHTN
{
    [Serializable]
    public class HRBCSNHTVHTNModel
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public string Ma_dvcs { get; set; }
        public string LoaiBC { get; set; }

        public List<HRBCSNHTVHTNRow> LsData { get; set; }

        public HRBCSNHTVHTNModel()
        {
            LsData = new List<HRBCSNHTVHTNRow>();
        }
    }

    // Dùng chung cho loại 0 (sinh nhật) và loại 1 (tròn năm)
    public class HRBCSNHTVHTNRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Ma_bpns { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public DateTime? Ngay_vao { get; set; }
        public int So_nam { get; set; }
    }

    // Dùng cho loại 2 (hết hạn HĐLĐ)
    public class HRBCSNHTVHTNHdldRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Ma_bpns { get; set; }
        public DateTime? Ngay_vao { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public string Ma_hdld { get; set; }
        public string Ten_lhdong { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
    }

    // Dùng cho loại 3 (hết hạn học việc/HDHV)
    public class HRBCSNHTVHTNHdhvRow
    {
        public int Stt { get; set; }
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Ma_bpns { get; set; }
        public string Ma_hdhv { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
    }
}
