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
    public class CustomIdentityUserDAL : ICustomIdentityUserDAL
    {
        private readonly CargoDbContext _context;
        public CustomIdentityUserDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(CustomIdentityUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CustomIdentityUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CustomIdentityUser>> GetAllAsync()
        {
            return await _context.Users.Include(nameof(CustomIdentityUser.Orders)).Include(nameof(CustomIdentityUser.FeedBacks)).ToListAsync();
        }

        public async Task<CustomIdentityUser> GetByIdAsync(string id)
        {
            return await _context.Users.Include(nameof(CustomIdentityUser.Orders)).Include(nameof(CustomIdentityUser.FeedBacks)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(CustomIdentityUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
