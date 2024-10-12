using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Abstracts
{
    public interface IFeedBackService
    {
        Task<List<FeedBack>> GetAllAsync();
        Task<FeedBack> GetByIdAsync(int id);
        Task AddAsync(FeedBack feedBack);
        Task UpdateAsync(FeedBack feedBack);
        Task DeleteAsync(FeedBack feedBack);
    }
}
