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
    public class CardDAL : ICardDAL
    {
        private readonly CargoDbContext _context;
        public CardDAL(CargoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Card card)
        {
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Card card)
        {
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Card>> GetAllAsync()
        {
            return await _context.Cards.ToListAsync();
        }
        public async Task<Card> GetByCardNumberAsync(string cardNumber, string bankName)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber && c.BankName == bankName);
        }
        public async Task<Card> GetByIdAsync(int id)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task UpdateAsync(Card card)
        {
            _context.Cards.Update(card);
            await _context.SaveChangesAsync();
        }
    }
}
