﻿using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface IChatService
    {
        Task<List<Chat>> GetAllAsync();
        Task<Chat> GetByIdAsync(int id);
        Task<Chat> GetBySenderIdAndReceiverIdAsync(string senderId, string receiverId)
        Task AddAsync(Chat chat);
        Task UpdateAsync(Chat chat);
        Task DeleteAsync(Chat chat);
    }
}
