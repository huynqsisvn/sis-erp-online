using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisCa.Pages.List.Cant
{
    [Serializable]
    [Table("dmnt")]
    public class Dmnt
    {
        [Key]
        [Column("ma_nt")]
        [Required]
        [StringLength(3)]
        public string Ma_nt { get; set; } = string.Empty;

        [Column("ten_nt")]
        [StringLength(250)]
        public string? Ten_nt { get; set; }

        [Column("ten_nt2")]
        [StringLength(250)]
        public string? Ten_nt2 { get; set; }

        [Column("ma_tra_cuu")]
        [StringLength(24)]
        public string? Ma_tra_cuu { get; set; }

        [Column("loai_tg")]
        [StringLength(1)]
        public string? Loai_tg { get; set; } = "1";

        [Column("status")]
        [StringLength(1)]
        public string? Status { get; set; } = "1";

        // Cách đọc tiếng Việt
        [Column("doc_vie")]
        [StringLength(24)]
        public string? Doc_vie { get; set; }

        [Column("begin1")]
        [StringLength(48)]
        public string? Begin1 { get; set; }

        [Column("point1")]
        [StringLength(48)]
        public string? Point1 { get; set; }

        [Column("endpoint1")]
        [StringLength(48)]
        public string? Endpoint1 { get; set; }

        [Column("end1")]
        [StringLength(48)]
        public string? End1 { get; set; }

        [Column("only1")]
        [StringLength(48)]
        public string? Only1 { get; set; }

        // Cách đọc tiếng Anh
        [Column("doc_eng")]
        [StringLength(1)]
        public string? Doc_eng { get; set; }

        [Column("begin2")]
        [StringLength(48)]
        public string? Begin2 { get; set; }

        [Column("point2")]
        [StringLength(48)]
        public string? Point2 { get; set; }

        [Column("endpoint2")]
        [StringLength(48)]
        public string? Endpoint2 { get; set; }

        [Column("end2")]
        [StringLength(48)]
        public string? End2 { get; set; }

        [Column("only2")]
        [StringLength(48)]
        public string? Only2 { get; set; }

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

        // TK fields (nếu có trong bảng)
        [Column("tk_pscl_no")]
        [StringLength(20)]
        public string? Tk_pscl_no { get; set; }

        [Column("tk_pscl_co")]
        [StringLength(20)]
        public string? Tk_pscl_co { get; set; }

        [Column("tk_dgcl_no")]
        [StringLength(20)]
        public string? Tk_dgcl_no { get; set; }

        [Column("tk_dgcl_co")]
        [StringLength(20)]
        public string? Tk_dgcl_co { get; set; }

        // Computed properties (không map vào DB)
        [NotMapped]
        public string StatusDisplay => Status == "1" ? "Đang dùng" : "Ngừng dùng";

        [NotMapped]
        public string LoaiTgDisplay => Loai_tg == "1" ? "Nhân với tỷ giá" : "Chia cho tỷ giá";
    }
}
