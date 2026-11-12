using System.Collections.Generic;

namespace SisFa.Pages.Input.Zcdmslts
{
    public class CtsltsPrintModel
    {
        public List<Ctslts> LSCt { get; set; } = new List<Ctslts>();
        public int Nam { get; set; }

        public int Ky { get; set; }
        public int Ky2 { get; set; }
        public string Ten_lien { get; set; }

        public string So_lien { get; set; }
    }
}
