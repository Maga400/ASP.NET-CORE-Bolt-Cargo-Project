using BoltCargo.Business.Services.Abstracts;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDAL _orderDAL;

        public OrderService(IOrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public async Task AddAsync(Order order)
        {
            await _orderDAL.AddAsync(order);
        }

        public async Task DeleteAsync(Order order)
        {
            await _orderDAL.DeleteAsync(order);
        }


        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderDAL.GetAllAsync();
        }
        public async Task<List<Order>> GetByCarTypeAsync(string carType)
        {
            return await _orderDAL.GetByCarTypeAsync(carType);
        }

        public async Task<List<Order>> GetByDriverIdAsync(string id)
        {
            return await _orderDAL.GetByDriverIdAsync(id);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderDAL.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetByUserIdAsync(string id)
        {
            return await _orderDAL.GetByUserIdAsync(id);
        }

        public async Task<List<Order>> GetAcceptedOrdersAsync()
        {
            return await _orderDAL.GetAcceptedOrdersAsync();
        }
        public async Task<List<Order>> GetUnAcceptedOrdersAsync()
        {
            return await _orderDAL.GetUnAcceptedOrdersAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _orderDAL.UpdateAsync(order);
        }
    }
}
