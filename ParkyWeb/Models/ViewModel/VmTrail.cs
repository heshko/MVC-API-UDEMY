using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ViewModel
{
    public class VmTrail
    {

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
