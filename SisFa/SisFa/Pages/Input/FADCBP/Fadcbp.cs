using System;
using System.ComponentModel.DataAnnotations;

namespace SisFa.Pages.Input.FADCBP
{
    public class Fadcbp
    {
        [Required(ErrorMessage = "Vui lòng nhập mã tài sản!")]
        public string So_the_ts { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm!")]
        public int Nam { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kỳ!")]
        [Range(1, 12, ErrorMessage = "Kỳ phải từ 1 đến 12!")]
        public int Ky { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã bộ phận sử dụng!")]
        public string Ma_bpts { get; set; }

        public string Tk_ts { get; set; }
        public string Tk_kh { get; set; }
        public string Tk_cp { get; set; }

        /// <summary>Cờ đánh dấu dữ liệu gốc. Giá trị 1 = dữ liệu khởi tạo gốc, không cho sửa/xóa.</summary>
        public string Ts0 { get; set; }

        // Audit trail - lần tạo
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }

        // Audit trail - lần sửa cuối
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }

        // Trường phụ trợ hiển thị STT trên grid
        public int Stt { get; set; }
    }
}

