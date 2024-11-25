using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface IComplaintDAL
    {
        Task<List<Complaint>> GetAllAsync();
        Task<Complaint> GetByIdAsync(int id);
        Task<int> GetReceiverComplaintsCountAsync(string receiverId);
        Task AddAsync(Complaint complaint);
        Task UpdateAsync(Complaint complaint);
        Task DeleteAsync(Complaint complaint);
    }
}
