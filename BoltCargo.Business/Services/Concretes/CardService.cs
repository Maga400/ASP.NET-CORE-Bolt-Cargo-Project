using BoltCargo.Business.Services.Abstracts;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.DataAccess.Repositories.Concretes;
using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Concretes
{
    public class CardService : ICardService
    {
        private readonly ICardDAL _cardDAL;
        public CardService(ICardDAL cardDAL)
        {
            _cardDAL = cardDAL;
        }
        public async Task AddAsync(Card card)
        {
           await _cardDAL.AddAsync(card);
        }
        public async Task DeleteAsync(Card card)
        {
            await _cardDAL.DeleteAsync(card);
        }
        public async Task<List<Card>> GetAllAsync()
        {
            return await _cardDAL.GetAllAsync();
        }
        public async Task<Card> GetByCardNumberAsync(string cardNumber, string bankName)
        {
            return await _cardDAL.GetByCardNumberAsync(cardNumber,bankName);
        }
        public async Task<Card> GetByIdAsync(int id)
        {
            return await _cardDAL.GetByIdAsync(id);
        }
        public async Task<Card> GetByUserIdAsync(string id)
        {
            return await _cardDAL.GetByUserIdAsync(id);
        }
        public async Task UpdateAsync(Card card)
        {
            await _cardDAL.UpdateAsync(card);
        }
    }
}
