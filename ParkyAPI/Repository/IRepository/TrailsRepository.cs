using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public class TrailsRepository : ITrailsRepository
    {

        private readonly ApplicationDbContext _context;

        public TrailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.Include(x => x.NationalPark).ToList();
        }
        public NationalPark GetTrailsInNationalPark(int npId)
        {
            var trailsInThisPark = _context.NationalParks.Include(x=>x.Trails).FirstOrDefault(x => x.Id == npId);
            return trailsInThisPark;
        }
        public bool TrailsExists(string name)
        {
            
            bool value = _context.Trails.Any(x => x.Name.ToLower().Trim() ==name);
            return value;
        }
        public bool TrailsExists(int id)
        {
            bool value = _context.Trails.Any(x => x.Id == id);
            return value;
        }

        public Trail GetTrailByName(string trailsName)
        {
            return _context.Trails.Include(x => x.NationalPark).FirstOrDefault(x => x.Name == trailsName);
        }
        public Trail GetTrailById(int trailsId)
        {
            return _context.Trails.Include(x => x.NationalPark).FirstOrDefault(x => x.Id == trailsId);
        }

        public bool CreateTrail(Trail trails)
        {
            _context.Trails.Add(trails);
            return Save();
        }
        public bool DeleteTrail(Trail trails)
        {
            _context.Trails.Remove(trails);
            return Save();
        }
        public bool UpdateTrail(Trail trails)
        {
            _context.Trails.Update(trails);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

       
    }
}
