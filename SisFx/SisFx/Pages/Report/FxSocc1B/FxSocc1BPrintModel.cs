using System;
using System.Collections.Generic;

namespace SisFx.Pages.Report.FxSocc1B
{
    [Serializable]
    public class FxSocc1BPrintModel
    {
        public int Nam { get; set; }
        public int Ky { get; set; }
        public DateTime? Ngay_giam_ts { get; set; }
        public DateTime? Ngay_ms { get; set; }
        public string Ma_bpsd { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ten_bpsd { get; set; }
        public string Ten_bpsd2 { get; set; }
        public string Ten_dvcs { get; set; }
        public string Ten_cty { get; set; }
        public string Dia_chi { get; set; }

        public List<FxSocc1BPrintRow> LsData { get; set; }

        /// <summary>??nh d?ng ngày m? s? dd/MM/yyyy cho báo cáo</summary>
        public string Ngay_ms_Formatted
        {
            get
            {
                return Ngay_ms.HasValue ? Ngay_ms.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string Ten_bpsd_Formatted
        {
            get
            {
                return $"{Ma_bpsd?.Trim()} - {Ten_bpsd}";
            }
        }

        public string Ten_bpsd_All
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Ten_bpsd2))
                    return Ten_bpsd;
                return $"{Ten_bpsd} / {Ten_bpsd2}";
            }
        }

        public FxSocc1BPrintModel()
        {
            LsData = new List<FxSocc1BPrintRow>();
        }
    }

    public class FxSocc1BPrintRow
    {
        public int? Stt { get; set; }
        public string Ma_cc_me { get; set; }
        public string Ten_cc { get; set; }
        public string Ten_cc2 { get; set; }
        public string Ma_cc { get; set; }
        public string So_ct { get; set; }
        public DateTime? Ngay_ct { get; set; }
        public string Dvt { get; set; }
        public decimal So_luong { get; set; }
        public decimal Nguyen_gia { get; set; }
        public decimal Tien { get; set; }
        public string So_ct_giam { get; set; }
        public DateTime? Ngay_giam { get; set; }
        public string Ma_giam_cc { get; set; }
        public decimal Sl_giam { get; set; }
        public decimal Tien_giam { get; set; }
        public string Ghi_chu { get; set; }
        public string So_hieu_cc { get; set; }
        public DateTime? Ngay_tang_cc { get; set; }
        public string Ly_do_tang_cc { get; set; }
        public string Nuoc_sx { get; set; }
        public DateTime? Ngay_mua { get; set; }
        public string Loai_pb { get; set; }
        public decimal Tl_pb { get; set; }
        public string Muc_pb { get; set; }
        public string Tk_pb { get; set; }
        public string Tk_cp { get; set; }
        public string Bp_da_tinh_kh { get; set; }
        public string Muc_kh { get; set; }
        public int So_ky_kh { get; set; }
        public decimal Tl_kh { get; set; }
        public decimal Gt_da_kh { get; set; }
        public decimal Gt_con_lai { get; set; }
        public string Chung_tu_giam { get; set; }
        public string Ly_do_giam_cc { get; set; }
        public string Ten_giam { get; set; }
        public string Ten_giam2 { get; set; }
        public int Thang_tinh_kh { get; set; }
        public int Nam_tinh_kh { get; set; }
        public string Nh_cc { get; set; }
        public string Nh_cc1 { get; set; }
        public string Nh_cc2 { get; set; }
        public string Nh_cc3 { get; set; }
        public string Loai_cc { get; set; }
        public string Ma_bpsd { get; set; }
        public string Ten_bpsd { get; set; }
        public string Ten_bpsd2 { get; set; }
        public DateTime? Ngay_ms { get; set; }
        public string Ma_dvcs { get; set; }
    }
}
