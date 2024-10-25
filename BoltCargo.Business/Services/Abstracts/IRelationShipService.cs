using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface IRelationShipService
    {
        Task<List<RelationShip>> GetAllAsync();
        Task<RelationShip> GetByIdAsync(int id);
        Task AddAsync(RelationShip relationShip);
        Task UpdateAsync(RelationShip relationShip);
        Task DeleteAsync(RelationShip relationShip);
    }
}
