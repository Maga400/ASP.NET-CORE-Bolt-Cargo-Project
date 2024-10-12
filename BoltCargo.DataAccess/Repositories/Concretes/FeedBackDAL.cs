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
    public class FeedBackDAL : IFeedBackDAL
    {
        private readonly CargoDbContext _context;
        public FeedBackDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(FeedBack feedBack)
        {
            await _context.FeedBacks.AddAsync(feedBack);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FeedBack feedBack)
        {
            _context.FeedBacks.Remove(feedBack);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeedBack>> GetAllAsync()
        {
            return await _context.FeedBacks.Include(nameof(FeedBack.User)).ToListAsync();
        }

        public async Task<FeedBack> GetByIdAsync(int id)
        {
            return await _context.FeedBacks.Include(nameof(FeedBack.User)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(FeedBack feedBack)
        {
            _context.FeedBacks.Update(feedBack);
            await _context.SaveChangesAsync();
        }
    }
}
