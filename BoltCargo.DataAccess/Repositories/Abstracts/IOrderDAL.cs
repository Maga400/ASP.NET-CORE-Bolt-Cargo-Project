using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface IOrderDAL
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetByUserIdAsync(string id);
        Task<List<Order>> GetByDriverIdAsync(string id);
        Task<List<Order>> GetByCarTypeAsync(string carType);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
