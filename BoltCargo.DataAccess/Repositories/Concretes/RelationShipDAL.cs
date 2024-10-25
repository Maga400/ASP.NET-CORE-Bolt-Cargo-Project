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
    public class RelationShipDAL : IRelationShipDAL
    {
        private readonly CargoDbContext _context;
        public RelationShipDAL(CargoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RelationShip relationShip)
        {
            await _context.RelationShips.AddAsync(relationShip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RelationShip relationShip)
        {
            _context.RelationShips.Remove(relationShip);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RelationShip>> GetAllAsync()
        {
            return await _context.RelationShips.ToListAsync();
        }

        public async Task<RelationShip> GetByIdAsync(int id)
        {
            return await _context.RelationShips.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(RelationShip relationShip)
        {
            _context.RelationShips.Update(relationShip);
            await _context.SaveChangesAsync();
        }
    }
}
