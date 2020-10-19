using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
  public  interface ITrailsRepository
    {

        ICollection<Trail> GetTrails();
         NationalPark  GetTrailsInNationalPark(int npId);

        Trail GetTrailById(int TrailsId);
        Trail GetTrailByName(string TrailsName);

        bool TrailsExists(string name);
        bool TrailsExists(int id);

        bool CreateTrail(Trail trails);

        bool UpdateTrail(Trail trails);

        bool DeleteTrail(Trail trails);

        bool Save();
    }
}
