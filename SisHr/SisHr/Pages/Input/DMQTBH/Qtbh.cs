using System;

namespace SisHr.Pages.Input.DMQTBH
{
    public class Qtbh
    {
        public string Qtbhid { get; set; }
        public string Ma_ns { get; set; }

        // BHXH
        public string So_so_bhxh { get; set; }
        public string So_the_bhxh { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
        public string Donvi_bh { get; set; }
        public string Donvi_cv { get; set; }

        // BHYT
        public string So_the_bhyt { get; set; }
        public DateTime? Bhyt_tungay { get; set; }
        public DateTime? Bhyt_denngay { get; set; }

        // Ghi chú / Tr?ng thái
        public string Ghi_chu { get; set; }
        public string Status { get; set; }

        // Audit fields (populated during insert/update)
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Display-only fields from view
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        public string Ngay_sinh { get; set; }
        public string Ngay_hdld { get; set; }
        public string Ten_cv { get; set; }
        public string Ten_cv2 { get; set; }
        public string Ma_hdld { get; set; }
        public string Ma_lhd { get; set; }

        // Extra fields loaded from dmns table (not in view)
        public string Cmnd { get; set; }
        public string Nc_cmnd { get; set; }
        public string Ngay_cmnd { get; set; }
        public string Ma_dantoc { get; set; }
        public string Quoc_tich { get; set; }
        public string Ho_khau { get; set; }
        public string Dia_chi { get; set; }
        public string Ngay_hl { get; set; }
    }
}

