
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisCom.Model
{
    public class FormatToolbarItem
    {
        public List<FormatToolbarItem> Childs;
        public bool? BeginGroup { get; set; }
        public string? Text { get; set; }
        public string? ValueName { get; set; }
        public int Id { get; set; }
        public int Index { get; set; } = 0;
        public int ParentId { get; set; }
        public string CssClass { get; set; }
        public string IconCssClass { get; set; }
        public string? Tooltip { get; set; }
        public string? ParentKey { get; set; }
       public bool? SplitDropDownButton { get; set; }
    }
}
