using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public class NationalParkRepository : INationalParkRepository
    {

        private readonly ApplicationDbContext _context;

        public NationalParkRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateNationalParkExists(NationalPark nationalPark)
        {
            _context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalParkExists(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool UpdateNationalParkExists(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _context.NationalParks.FirstOrDefault(x => x.Id == nationalParkId);
        }
        public NationalPark GetNationalPark(string   nationalParkName)
        {
            return _context.NationalParks.FirstOrDefault(x => x.Name == nationalParkName);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalParks.OrderBy(x=>x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _context.NationalParks.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            bool value = _context.NationalParks.Any(x => x.Id == id);
            return value;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

      
    }
}
