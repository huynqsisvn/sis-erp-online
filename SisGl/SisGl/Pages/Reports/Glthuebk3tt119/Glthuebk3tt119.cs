namespace SisGl.Pages.Reports.Glthuebk3tt119
{
    [Serializable]
    public class Glthuebk3tt119Model
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string LoaiBC { get; set; }
        public string Ma_dvcs { get; set; }

        public List<Glthuebk3tt119> LsData { get; set; }
        public Glthuebk3tt119Model()
        {
            LsData = new List<Glthuebk3tt119>();
        }
    }
    public class Glthuebk3tt119
    {
        public int Stt { get; set; }

        public string So_ct0 { get; set; }          // Số hđ
        public string Kh_mau_hd { get; set; }       // Mẫu hđ
        public string So_seri0 { get; set; }        // Ký hiệu hđ
        public DateTime? Ngay_ct0 { get; set; }     // Ngày hđ

        public string Ma_kh { get; set; }           // Mã người bán
        public string Ten_kh { get; set; }          // Tên người bán
        public string Dia_chi { get; set; }         // Địa chỉ
        public string Ma_so_thue { get; set; }      // Mã số thuế

        public string Ten_vt { get; set; }          // H.hóa, d.vụ

        public decimal T_tien { get; set; }         // Gt mua chưa thuế
        public string Ma_thue { get; set; }         // Mã thuế
        public decimal Thue_suat { get; set; }      // Thuế suất %
        public decimal T_thue { get; set; }         // Thuế GTGT
        public decimal T_tt { get; set; }           

        public string Ghi_chu { get; set; }

        public string Tk_thue_no { get; set; }      // Tk thuế
        public string Tk_du { get; set; }           // Tk đối ứng

        public string Ma_vv { get; set; }           // Mã dự án
        public string Ma_hdm { get; set; }          // Số đh mua
        public string Ma_hd { get; set; }           // Số đh bán

        public string Ma_dvcs { get; set; }         // Mã ĐVCS
        public string Ma_ct { get; set; }           // Mã c.từ
        public DateTime? Ngay_ct { get; set; }      // Ngày c.từ
        public string Ma_qs { get; set; }           // Quyển c.từ
        public string So_ct1 { get; set; }          // Số c.từ

        public string Mau_so_tt119 { get; set; }    // Nhóm hđ TT119
        public string Ma_mstmp { get; set; }        // Nhóm hđ
    }
}

