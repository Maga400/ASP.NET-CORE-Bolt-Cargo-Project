using BoltCargo.DataAccess.Data;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Concretes
{
    public class PriceDAL : IPriceDAL
    {
        private readonly CargoDbContext _context;
        public PriceDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Price price)
        {
            await _context.Prices.AddAsync(price);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Price price)
        {
            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Price>> GetAllAsync()
        {
            return await _context.Prices.ToListAsync();
        }

        public async Task<Price> GetByIdAsync(int id)
        {
            return await _context.Prices.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Price price)
        {
            _context.Prices.Update(price);
            await _context.SaveChangesAsync();
        }
    }
}
