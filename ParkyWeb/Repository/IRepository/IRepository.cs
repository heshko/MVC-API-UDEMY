using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
   public interface IRepository<T> where T : class
    {

        Task<IList<T>> GetAllAsync(string url);

        Task<T> GetAsync(string url,int id);

        Task<bool> CreateAsync(string url, T CreateObject);

        Task<bool> UpdateAsync(string url, T CreateObject);

        Task<bool> DeleteAsync(string url,int id);
    }
}
