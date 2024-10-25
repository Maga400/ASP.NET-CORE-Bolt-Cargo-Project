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
    public class RelationShipRequestDAL : IRelationShipRequestDAL
    {
        private readonly CargoDbContext _context;
        public RelationShipRequestDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RelationShipRequest request)
        {
            await _context.RelationShipRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RelationShipRequest request)
        {
            _context.RelationShipRequests.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RelationShipRequest>> GetAllAsync()
        {
            return await _context.RelationShipRequests.ToListAsync();    
        }

        public async Task<RelationShipRequest> GetByIdAsync(int id)
        {
            return await _context.RelationShipRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(RelationShipRequest request)
        {
            _context.RelationShipRequests.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
