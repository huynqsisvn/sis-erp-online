using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Model
{
    public class MenuTextFormatting
    {
        public  int Stt { get; set; }
        public string Menu_id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string IconCssClass { get; set; }
        public string CssClass { get; set; }
        public string NavigateUrl { get; set; }
        public bool? BeginGroup { get; set; }

    }
}
