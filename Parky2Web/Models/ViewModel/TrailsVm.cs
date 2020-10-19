using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parky2Web.Models.ViewModel
{
    public class TrailsVm
    {
       
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListNationalParks { get; set; }
       
        public Trail Trail { get; set; }
    }
}
