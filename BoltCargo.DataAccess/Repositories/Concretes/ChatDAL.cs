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
    public class ChatDAL : IChatDAL
    {
        private readonly CargoDbContext _context;
        public ChatDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Chat chat)
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _context.Chats.Include(nameof(Chat.Messages)).Include(nameof(Chat.Receiver)).ToListAsync();
        }

        public async Task<Chat> GetBySenderIdAndReceiverIdAsync(string senderId,string receiverId)
        {
            return await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == senderId && c.ReceiverId == receiverId || c.ReceiverId == senderId && c.SenderId == receiverId);
        }
        public async Task<Chat> GetByIdAsync(int id)
        {
            return await _context.Chats.Include(nameof(Chat.Messages)).Include(nameof(Chat.Receiver)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }
    }
}
