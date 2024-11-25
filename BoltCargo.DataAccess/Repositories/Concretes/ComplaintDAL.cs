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
    public class ComplaintDAL : IComplaintDAL
    {
        private readonly CargoDbContext _context;
        public ComplaintDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Complaint complaint)
        {
            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Complaint complaint)
        {
            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Complaint>> GetAllAsync()
        {
            return await _context.Complaints.ToListAsync();
        }
        public async Task<Complaint> GetByIdAsync(int id)
        {
            return await _context.Complaints.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> GetReceiverComplaintsCountAsync(string receiverId)
        {
            return await _context.Complaints.Where(c => c.ReceiverId == receiverId).CountAsync();
        }
        public async Task UpdateAsync(Complaint complaint)
        {
            _context.Complaints.Update(complaint);
            await _context.SaveChangesAsync();
        }
    }
}
