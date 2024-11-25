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
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintDAL _complaintDAL;
        public ComplaintService(IComplaintDAL complaintDAL)
        {
            _complaintDAL = complaintDAL;
        }
        public async Task AddAsync(Complaint complaint)
        {
            await _complaintDAL.AddAsync(complaint);
        }
        public async Task DeleteAsync(Complaint complaint)
        {
            await _complaintDAL.DeleteAsync(complaint);
        }
        public async Task<List<Complaint>> GetAllAsync()
        {
            return await _complaintDAL.GetAllAsync();
        }

        public async Task<Complaint> GetByIdAsync(int id)
        {
            return await _complaintDAL.GetByIdAsync(id);
        }
        public async Task<int> GetReceiverComplaintsCountAsync(string receiverId)
        {
            return await _complaintDAL.GetReceiverComplaintsCountAsync(receiverId);
        }
        public async Task UpdateAsync(Complaint complaint)
        {
            await _complaintDAL.UpdateAsync(complaint);
        }
    }
}
