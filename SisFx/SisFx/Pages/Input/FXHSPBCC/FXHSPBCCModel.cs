using System;
using System.Collections.Generic;

namespace SisFx.Pages.Input.FXHSPBCC
{
    /// <summary>
    /// Model Header — phiếu hệ số phân bổ chi phí CCDC (hspbcc)
    /// Tương đương tbMain trong WinForm StartUp.cs (Tables[0])
    /// </summary>
    public class PhHspbcc
    {
        public int      Stt       { get; set; }
        public string   Ma_cc_me  { get; set; } = "";   // Mã công cụ (composite key 1)
        public string   Ma_bpcc   { get; set; } = "";   // Mã BPSD (composite key 2)
        public string   Ma_dvcs   { get; set; } = "";

        // Join từ dmcc / dmbpts để hiển thị
        public string   Ten_cc    { get; set; } = "";
        public string   Ten_cc2   { get; set; } = "";
        public string   Ten_bpcc  { get; set; } = "";
        public string   Ten_bpcc2 { get; set; } = "";
        public string   Tk_cp     { get; set; } = "";   // Tk chi phí từ dmcc
        public string   Ma_phi    { get; set; } = "";
    }

    /// <summary>
    /// Model Detail — chi tiết hệ số phân bổ (hspbcc)
    /// Tương đương tbDetail trong WinForm StartUp.cs (Tables[1])
    /// Ánh xạ newForm.xaml.cs: GrdCt các cột tk_cp, ma_bpht, ma_px, ma_vv, so_lsx, ma_sp,
    ///     ngay_hl1, ngay_hl2, he_so
    /// </summary>
    public class CtBpcc
    {
        public int      Stt       { get; set; }
        public string   Ma_cc_me  { get; set; } = "";
        public string   Ma_bpcc   { get; set; } = "";
        public string   Ma_dvcs   { get; set; } = "";

        public string   Tk_cp     { get; set; } = "";   // Tài khoản chi phí
        public string   Ma_bpht   { get; set; } = "";   // Mã bộ phận hạch toán
        public string   Ma_px     { get; set; } = "";   // Mã phân xưởng
        public string   Ma_vv     { get; set; } = "";   // Mã dự án
        public string   So_lsx    { get; set; } = "";   // Số lệnh sản xuất
        public string   Ma_sp     { get; set; } = "";   // Mã sản phẩm

        public DateTime? Ngay_hl1 { get; set; }         // Hiệu lực từ ngày
        public DateTime? Ngay_hl2 { get; set; }         // Hiệu lực đến ngày

        public decimal  He_so     { get; set; }         // Hệ số

        // Trường hệ thống
        public DateTime? Date0    { get; set; }
        public string   Time0     { get; set; } = "";
        public string   User_id0  { get; set; } = "";
    }
}
