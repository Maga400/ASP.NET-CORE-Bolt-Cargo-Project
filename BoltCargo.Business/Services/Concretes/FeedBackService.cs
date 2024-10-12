using BoltCargo.Business.Services.Abstracts;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Business.Services.Concretes
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackDAL _feedBackDAL;

        public FeedBackService(IFeedBackDAL feedBackDAL)
        {
            _feedBackDAL = feedBackDAL;
        }

        public async Task AddAsync(FeedBack feedBack)
        {
            await _feedBackDAL.AddAsync(feedBack);
        }

        public async Task DeleteAsync(FeedBack feedBack)
        {
            await _feedBackDAL.DeleteAsync(feedBack);
        }

        public async Task<List<FeedBack>> GetAllAsync()
        {
            return await _feedBackDAL.GetAllAsync();
        }

        public async Task<FeedBack> GetByIdAsync(int id)
        {
            return await _feedBackDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(FeedBack feedBack)
        {
            await _feedBackDAL.UpdateAsync(feedBack);
        }
    }
}
