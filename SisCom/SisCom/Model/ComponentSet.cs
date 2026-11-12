using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Model
{
    public class ComponentSet
    {
        public string Menu_id0 { get; set; }
        public string Menu_id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NavigateUrl { get; set; }
        public string IconUrl { get; set; }
        public string CssItem { get; set; }
        public string Cssclass { get; set; }
        public List<ComponentSet> ComponentSets { get; set; }
    }
}
