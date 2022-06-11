using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    internal interface IDatabase<T>
    {
        Task<List<T>> GetListAsync();
        Task<T> GetAsync(int id);
        Task<int> SaveAsync(T t);
        Task<int> DeleteAsync(T t);
    }
}
