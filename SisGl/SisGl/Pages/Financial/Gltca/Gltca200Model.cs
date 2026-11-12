namespace SisGl.Pages.Financial.Gltca
{
    [Serializable]
    public class Gltca200Model
    {
        public DateTime Tu_ngay { get; set; }
        public DateTime Den_ngay { get; set; }
        public DateTime Ngay_dkt { get; set; }
        public DateTime Ngay_ckt { get; set; }
        public DateTime Ngay_bd_lk { get; set; }
        public string MaDVCS { get; set; }
        public string Ma_nt0 { get; set; }
        public int Mau_bc { get; set; }
        public List<Gltca200> LsData { get; set; }
    }

    public class Gltca200
    {
        public int No { get; set; }
        public Decimal Stt { get; set; }
        public Decimal Bold { get; set; }
        public Decimal In_ck { get; set; }
        public String Cach_tinh { get; set; }
        public String Ma_so { get; set; }
        public String Chi_tieu { get; set; }
        public String Chi_tieu2 { get; set; }
        public String Th_minh { get; set; }
        public String Kind { get; set; }
        public String Tk_no { get; set; }
        public String Tk_co { get; set; }
        public Decimal Giam_tru { get; set; }
        public Decimal Khong_am { get; set; }
        public Decimal Ky_truoc { get; set; }
        public Decimal Ky_nay { get; set; }
        public Decimal Luy_ke { get; set; }
        public Decimal Ky_truocnt { get; set; }
        public Decimal Ky_nay_nt { get; set; }
        public Decimal Luy_ke_nt { get; set; }
        public Decimal Lk_kt { get; set; }
        public Decimal Lk_kt_nt { get; set; }
        public Decimal Ty_le { get; set; }
        public Decimal Ty_le_nt { get; set; }
        public Decimal Ty_le_lk { get; set; }
        public Decimal Ty_le_lk_nt { get; set; }
    }
}
