using SisGl.Model;

namespace SisGl.Pages.Financial.Gltcb
{
    [Serializable]
    public class Gltcb200Model
    {
        public DateTime Tu_ngay { get; set; }
        public DateTime Den_ngay { get; set; }
        public string Tu_ngay_Den_ngay { get; set; }
        public string MaDVCS { get; set; }
        public string Dvt { get; set; }
        public int Mau_bc { get; set; }
        public List<Gltcb200> LsData { get; set; }

    }
}
