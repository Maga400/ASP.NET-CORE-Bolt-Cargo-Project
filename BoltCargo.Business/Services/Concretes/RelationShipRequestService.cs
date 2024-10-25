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
    public class RelationShipRequestService : IRelationShipRequestService
    {
        private readonly IRelationShipRequestDAL _relationShipRequestDAL;
        public RelationShipRequestService(IRelationShipRequestDAL relationShipRequestDAL)
        {
            _relationShipRequestDAL = relationShipRequestDAL;
        }
        public async Task AddAsync(RelationShipRequest request)
        {
            await _relationShipRequestDAL.AddAsync(request);
        }
        public async Task DeleteAsync(RelationShipRequest request)
        {
            await _relationShipRequestDAL.DeleteAsync(request);
        }
        public async Task<List<RelationShipRequest>> GetAllAsync()
        {
            return await _relationShipRequestDAL.GetAllAsync();
        }
        public async Task<RelationShipRequest> GetByIdAsync(int id)
        {
            return await _relationShipRequestDAL.GetByIdAsync(id);
        }
        public async Task UpdateAsync(RelationShipRequest request)
        {
            await _relationShipRequestDAL.UpdateAsync(request);
        }
    }
}
