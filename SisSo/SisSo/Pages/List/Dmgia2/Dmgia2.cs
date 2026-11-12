using System;

namespace SisSo.Pages.List.Dmgia2
{
    [Serializable]
    public class Dmgia2
    {
        public string Ma_vt { get; set; }
        public string Ma_kh { get; set; }
        public DateTime? Ngay_ban { get; set; }
        public string Ma_gia2 { get; set; }
        public decimal Gia2 { get; set; }
        public decimal Gia_nt2 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string Status { get; set; }
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        public decimal? Sl_td1 { get; set; }
        public decimal? Sl_td2 { get; set; }
        public decimal? Sl_td3 { get; set; }
        public string Gc_td1 { get; set; }
        public string Gc_td2 { get; set; }
        public string Gc_td3 { get; set; }
        public string User_name { get; set; }
        public string User_name0 { get; set; }
        public string Search { get; set; }
        public string Nh_kh3 { get; set; }
        public int? User_id { get; set; }
        public string Giaid { get; set; }

        // Các trường mở rộng để hiển thị
        public string Ten_vt { get; set; }
        public string Ten_vt2 { get; set; }
        public string Ten_kh { get; set; }
        public string Dvt { get; set; }
        public string Ten_gia2 { get; set; }
        public string Ten_nh_kh3 { get; set; }
    }
}
