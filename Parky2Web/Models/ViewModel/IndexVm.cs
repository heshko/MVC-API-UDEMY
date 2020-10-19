using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parky2Web.Models.ViewModel
{
    public class IndexVm
    {

        public IEnumerable<NationalPark> NationalParkList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
    }
}
