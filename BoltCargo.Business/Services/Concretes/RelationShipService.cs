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
    public class RelationShipService : IRelationShipService
    {
        private readonly IRelationShipDAL _relationShipDAL;
        public RelationShipService(IRelationShipDAL relationShipDAL)
        {
            _relationShipDAL = relationShipDAL;
        }
        public async Task AddAsync(RelationShip relationShip)
        {
            await _relationShipDAL.AddAsync(relationShip);
        }
        public async Task DeleteAsync(RelationShip relationShip)
        {
            await _relationShipDAL.DeleteAsync(relationShip);
        }
        public async Task<List<RelationShip>> GetAllAsync()
        {
            return await _relationShipDAL.GetAllAsync();
        }
        public async Task<RelationShip> GetByIdAsync(int id)
        {
            return await _relationShipDAL.GetByIdAsync(id);
        }
        public async Task UpdateAsync(RelationShip relationShip)
        {
            await _relationShipDAL.UpdateAsync(relationShip);
        }
    }
}
