using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisCa.Pages.List.Catknh
{
    [Serializable]
    [Table("dmtknh")]
    public class Dmtknh
    {
        [Column("tk")]
        [Required]
        public string Tk { get; set; } = string.Empty;

        [Column("tk_nh")]
        public string? Tk_nh { get; set; }

        [Column("ten_nh")]
        public string? Ten_nh { get; set; }

        [Column("ten_nh2")]
        public string? Ten_nh2 { get; set; }

        [Column("tinh_thanh")]
        public string? Tinh_thanh { get; set; }

        [Column("dien_thoai")]
        public string? Dien_thoai { get; set; }

        [Column("fax")]
        public string? Fax { get; set; }

        [Column("e_mail")]
        public string? E_mail { get; set; }

        [Column("home_page")]
        public string? Home_page { get; set; }

        [Column("doi_tac")]
        public string? Doi_tac { get; set; }

        [Column("ma_so_thue")]
        public string? Ma_so_thue { get; set; }

        [Column("ghi_chu")]
        public string? Ghi_chu { get; set; }

        [Column("status")]
        public string? Status { get; set; } = "1";

        [Column("ma_tra_cuu")]
        public string? Ma_tra_cuu { get; set; }

        // Free fields
        [Column("ma_td1")]
        public string? Ma_td1 { get; set; }

        [Column("ma_td2")]
        public string? Ma_td2 { get; set; }

        [Column("ma_td3")]
        public string? Ma_td3 { get; set; }

        [Column("ngay_td1")]
        public DateTime? Ngay_td1 { get; set; }

        [Column("ngay_td2")]
        public DateTime? Ngay_td2 { get; set; }

        [Column("ngay_td3")]
        public DateTime? Ngay_td3 { get; set; }

        [Column("sl_td1")]
        public decimal? Sl_td1 { get; set; }

        [Column("sl_td2")]
        public decimal? Sl_td2 { get; set; }

        [Column("sl_td3")]
        public decimal? Sl_td3 { get; set; }

        [Column("gc_td1")]
        public string? Gc_td1 { get; set; }

        [Column("gc_td2")]
        public string? Gc_td2 { get; set; }

        [Column("gc_td3")]
        public string? Gc_td3 { get; set; }

        // Audit fields
        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("time")]
        public string? Time { get; set; }

        [Column("user_id")]
        public int? User_id { get; set; }

        [Column("user_name")]
        public string? User_name { get; set; }

        [Column("date0")]
        public DateTime? Date0 { get; set; }

        [Column("time0")]
        [StringLength(20)]
        public string? Time0 { get; set; }

        [Column("user_id0")]
        public int? User_id0 { get; set; }

        [Column("user_name0")]
        public string? User_name0 { get; set; }

        [Column("search")]
        public string? Search { get; set; }
      
    }
}
