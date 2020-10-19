using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parky2Web.Repository.IRepository
{
   public interface IRepository<T> where T:class
    {

        Task<List<T>> GetAllAsync(string url);
        Task<T> GetByIdAsync(int id, string url);

        Task<bool> CreateAsync(T obj, string url);
        Task<bool> UpdateAsync(T obj, string url);
        Task<bool> DeleteAsync(int id, string url);


    }
}
