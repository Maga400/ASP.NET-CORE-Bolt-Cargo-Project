using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetByUserIdAsync(string id);
        Task<List<Order>> GetClientFinishedOrdersAsync(string id);
        Task<List<Order>> GetByDriverIdAsync(string id);
        Task<List<Order>> GetDriverFinishedOrdersAsync(string id);
        Task<List<Order>> GetByCarTypeAsync(string carType);
        Task<List<Order>> GetAcceptedOrdersAsync();
        Task<List<Order>> GetUnAcceptedOrdersAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
