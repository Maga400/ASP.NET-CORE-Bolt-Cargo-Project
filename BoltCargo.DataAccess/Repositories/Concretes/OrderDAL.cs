using BoltCargo.DataAccess.Data;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Concretes
{
    public class OrderDAL : IOrderDAL
    {
        private readonly CargoDbContext _context;
        public OrderDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(nameof(Order.User)).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(nameof(Order.User)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetByUserIdAsync(string id)
        {
            return await _context.Orders.Include(nameof(Order.User)).Where(o => o.UserId == id).ToListAsync();
        }
        public async Task<List<Order>> GetByDriverIdAsync(string id) 
        {
            return await _context.Orders.Include(nameof(Order.User)).Where(o => o.DriverId == id).ToListAsync();
        }
         
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetByCarTypeAsync(string carType)
        {
            return await _context.Orders.Include(nameof(Order.User)).Where(o => o.CarType == carType && string.IsNullOrEmpty(o.DriverId)).ToListAsync();
        }

        public async Task<List<Order>> GetAcceptedOrdersAsync()
        {
            return await _context.Orders.Include(nameof(Order.User)).Where(o => o.IsAccept == true).ToListAsync();
        }

        public async Task<List<Order>> GetUnAcceptedOrdersAsync()
        {
            return await _context.Orders.Include(nameof(Order.User)).Where(o => o.IsAccept == false).ToListAsync();

        }
    }
}
