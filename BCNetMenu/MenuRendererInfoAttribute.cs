using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCNetMenu
{
    public class MenuRendererDescriptionAttribute : Attribute
    {
        public String Description { get; set; }
        public MenuRendererDescriptionAttribute(String pDescription)
        {
            this.Description = pDescription;
        }
    }
}
