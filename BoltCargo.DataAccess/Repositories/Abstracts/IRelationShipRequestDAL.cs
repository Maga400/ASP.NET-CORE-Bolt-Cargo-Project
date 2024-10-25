using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface IRelationShipRequestDAL
    {
        Task<List<RelationShipRequest>> GetAllAsync();
        Task<RelationShipRequest> GetByIdAsync(int id);
        Task AddAsync(RelationShipRequest request);
        Task UpdateAsync(RelationShipRequest request);
        Task DeleteAsync(RelationShipRequest request);
    }
}
