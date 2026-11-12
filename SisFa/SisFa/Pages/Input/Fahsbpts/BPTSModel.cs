using System;

namespace SisFa.Pages.Input.Fahsbpts
{
    /// <summary>
    /// Model Header - B?ng phân b? h? s? kh?u hao TSC? (ph_hspbts)
    /// </summary>
    public class PhHspbts
    {
        public int Stt { get; set; }            // S? th? t? (UI only, ROW_NUMBER)
        public string So_the_ts { get; set; }
        public string Ma_bpts { get; set; }
        public string Ma_dvcs { get; set; }
        public int Nam { get; set; }
        public int Ky { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string User_id { get; set; }

        // Virtual fields - hi?n th? UI, join t? danh m?c
        public string Loai_ts { get; set; }
        public string Ten_ts { get; set; }
        public string Ten_ts2 { get; set; }
        public string Ten_bpts { get; set; }
        public string Ten_bpts2 { get; set; }
        public string Tk_khao { get; set; }
        public string Ma_phi { get; set; }
    }

    /// <summary>
    /// Model Detail - B?ng chi ti?t h? s? phân b? (hspbts)
    /// </summary>
    public class CtBpts
    {
        public int Stt { get; set; }            // S? th? t? (UI only)
        public string So_the_ts { get; set; }
        public string Ma_bpts { get; set; }
        public string Ma_dvcs { get; set; }     // Mã ??n v? c? s?

        public string Tk_cp { get; set; }       // Tk cp
        public string Ma_bpht { get; set; }     // Mã bpht
        public string Ma_px { get; set; }       // Mã px
        public string Ma_vv { get; set; }       // Mã d? án
        public string So_lsx { get; set; }      // S? lsx
        public string Ma_sp { get; set; }       // Mã s?n ph?m

        public DateTime? Ngay_hl1 { get; set; } // Hi?u l?c t? ngày
        public DateTime? Ngay_hl2 { get; set; } // Hi?u l?c ??n ngày

        public decimal He_so { get; set; }      // H? s?

        // Các tr??ng h? th?ng
        public decimal Tien { get; set; }
        public decimal Tien_nt { get; set; }
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public string User_id0 { get; set; }
        public string Status { get; set; }
    }
}
