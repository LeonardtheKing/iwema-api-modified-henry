using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentsWithChildren
{
    public class GetChildrenQueryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
