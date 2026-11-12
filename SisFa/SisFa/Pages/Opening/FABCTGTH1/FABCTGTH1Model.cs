using System;
using System.Collections.Generic;

namespace SisFa.Pages.Opening.FABCTGTH1
{
    // Bảng này đóng vai trò tương đương bảng "dien_giai" trong DataSet cũ
    [Serializable]
    public class FABCTGTH1Model
    {
        public int Nam { get; set; }
        public int TuKy { get; set; }
        public int DenKy { get; set; }
        public string v_title_report { get; set; }
        public string e_title_report { get; set; }
        public string value_detail_report { get; set; }
        public string v_tuKydenKy { get; set; }
        public string e_tuKydenKy { get; set; }

        // LƯU Ý QUAN TRỌNG: Tên biến phải là tbDetail để DevExpress map đúng DataMember = "tbDetail"
        public List<FABCTGTH1Row> tbDetail { get; set; }

        public FABCTGTH1Model()
        {
            tbDetail = new List<FABCTGTH1Row>();
        }
    }

    [Serializable]
    public class FABCTGTH1Row
    {
        // --- CÁC CỘT CỐ ĐỊNH ---
        public string khoa { get; set; }
        public string so_the_ts { get; set; }
        public string ma_tg_ts { get; set; }
        public string stt { get; set; }
        public string chi_tieu { get; set; }
        public string chi_tieu2 { get; set; }
        public int? bold { get; set; }
        public string ftag { get; set; }
        public string ftage { get; set; }
        public decimal? T_L_ { get; set; } // Cột tổng cộng

        // --- CÁC CỘT PIVOT ĐỘNG (Dự phòng lên tới 30 loại tài sản) ---
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