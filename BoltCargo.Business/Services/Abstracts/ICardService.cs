using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface ICardService
    {
        Task<List<Card>> GetAllAsync();
        Task<Card> GetByIdAsync(int id);
        Task<Card> GetByCardNumberAsync(string cardNumber, string bankName);
        Task AddAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(Card card);
    }
}
