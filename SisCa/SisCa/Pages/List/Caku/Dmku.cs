using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisCa.Pages.List.Caku
{
    [Serializable]
    [Table("dmku")]
    public class Dmku
    {
        [Key]
        [Column("ma_ku")]
        [Required]
        [StringLength(24)]
        public string Ma_ku { get; set; } = string.Empty;

        [Column("so_ku")]
        [StringLength(24)]
        public string? So_ku { get; set; }

        [Column("ten_ku")]
        [StringLength(250)]
        public string? Ten_ku { get; set; }

        [Column("ten_ku2")]
        [StringLength(250)]
        public string? Ten_ku2 { get; set; }

        [Column("ma_ku_me")]
        [StringLength(24)]
        public string? Ma_ku_me { get; set; }

        [Column("ma_kh")]
        [StringLength(24)]
        public string? Ma_kh { get; set; }

        [Column("ngay_ku")]
        public DateTime? Ngay_ku { get; set; }

        [Column("sn_vay")]
        public int? Sn_vay { get; set; }

        [Column("loai_ku")]
        [StringLength(1)]
        public string? Loai_ku { get; set; } = "1";

        [Column("tinh_tra")]
        public int? Tinh_tra { get; set; } = 0;

        [Column("ngay_ku1")]
        public DateTime? Ngay_ku1 { get; set; }

        [Column("lai_suat1")]
        public decimal? Lai_suat1 { get; set; }

        [Column("ngay_ku2")]
        public DateTime? Ngay_ku2 { get; set; }

        [Column("lai_suat2")]
        public decimal? Lai_suat2 { get; set; }

        [Column("tk")]
        [StringLength(20)]
        public string? Tk { get; set; }

        [Column("ma_nt")]
        [StringLength(3)]
        public string? Ma_nt { get; set; }

        [Column("tien0")]
        public decimal? Tien0 { get; set; }

        [Column("tien_nt0")]
        public decimal? Tien_nt0 { get; set; }

        [Column("ma_vv")]
        [StringLength(24)]
        public string? Ma_vv { get; set; }

        [Column("ma_hd")]
        [StringLength(24)]
        public string? Ma_hd { get; set; }

        [Column("ma_hdm")]
        [StringLength(24)]
        public string? Ma_hdm { get; set; }

        [Column("nh_ku1")]
        [StringLength(24)]
        public string? Nh_ku1 { get; set; }

        [Column("nh_ku2")]
        [StringLength(24)]
        public string? Nh_ku2 { get; set; }

        [Column("nh_ku3")]
        [StringLength(24)]
        public string? Nh_ku3 { get; set; }

        [Column("ma_td1")]
        [StringLength(24)]
        public string? Ma_td1 { get; set; }

        [Column("ma_td2")]
        [StringLength(24)]
        public string? Ma_td2 { get; set; }

        [Column("ma_td3")]
        [StringLength(24)]
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
        [StringLength(250)]
        public string? Gc_td1 { get; set; }

        [Column("gc_td2")]
        [StringLength(250)]
        public string? Gc_td2 { get; set; }

        [Column("gc_td3")]
        [StringLength(250)]
        public string? Gc_td3 { get; set; }

        [Column("ma_tra_cuu")]
        [StringLength(24)]
        public string? Ma_tra_cuu { get; set; }

        [Column("ghi_chu")]
        [StringLength(500)]
        public string? Ghi_chu { get; set; }

        [Column("status")]
        [StringLength(1)]
        public string? Status { get; set; } = "1";

        [Column("status1")]
        [StringLength(1)]
        public string? Status1 { get; set; }

        // Audit fields
        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("time")]
        [StringLength(20)]
        public string? Time { get; set; }

        [Column("user_id")]
        public int? User_id { get; set; }

        [Column("user_name")]
        [StringLength(50)]
        public string? User_name { get; set; }

        [Column("date0")]
        public DateTime? Date0 { get; set; }

        [Column("time0")]
        [StringLength(20)]
        public string? Time0 { get; set; }

        [Column("user_id0")]
        public int? User_id0 { get; set; }

        [Column("user_name0")]
        [StringLength(50)]
        public string? User_name0 { get; set; }

        [Column("search")]
        [StringLength(500)]
        public string? Search { get; set; }

        // Computed properties (không map vào DB)
        [NotMapped]
        public string StatusDisplay => Status == "1" ? "Sử dụng" : "Không sử dụng";

        [NotMapped]
        public string TinhTraDisplay => Tinh_tra == 0 ? "Không" : "Có";

        [NotMapped]
        public string Ten_kh { get; set; }
    }
}
