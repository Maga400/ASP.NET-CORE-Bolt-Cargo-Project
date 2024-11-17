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
    public class PriceService : IPriceService
    {
        private readonly IPriceDAL _priceDAL;
        public PriceService(IPriceDAL priceDAL)
        {
            _priceDAL = priceDAL;
        }

        public async Task AddAsync(Price price)
        {
            await _priceDAL.AddAsync(price);
        }
        public async Task DeleteAsync(Price price)
        {
            await _priceDAL.DeleteAsync(price);
        }

        public async Task<List<Price>> GetAllAsync()
        {
            return await _priceDAL.GetAllAsync();
        }

        public async Task<Price> GetByIdAsync(int id)
        {
            return await _priceDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Price price)
        {
            await _priceDAL.UpdateAsync(price);
        }
    }
}
