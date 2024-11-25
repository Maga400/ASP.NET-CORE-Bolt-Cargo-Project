﻿using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface ICardDAL
    {
        Task<List<Card>> GetAllAsync();
        Task<Card> GetByIdAsync(int id);
        Task<Card> GetByUserIdAsync(string id);
        Task<Card> GetByCardNumberAsync(string cardNumber,string bankName);
        Task AddAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(Card card);
    }
}
