using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface IPriceDAL 
    {
        Task<List<Price>> GetAllAsync();
        Task<Price> GetByIdAsync(int id);
        Task AddAsync(Price price);
        Task UpdateAsync(Price price);
        Task DeleteAsync(Price price);
    }
}
