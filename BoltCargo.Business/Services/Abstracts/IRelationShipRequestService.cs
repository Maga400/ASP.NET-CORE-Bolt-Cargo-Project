using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface IRelationShipRequestService
    {
        Task<List<RelationShipRequest>> GetAllAsync();
        Task<RelationShipRequest> GetByIdAsync(int id);
        Task AddAsync(RelationShipRequest request);
        Task UpdateAsync(RelationShipRequest request);
        Task DeleteAsync(RelationShipRequest request);
    }
}
