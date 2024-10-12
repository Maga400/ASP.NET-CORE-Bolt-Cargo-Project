using BoltCargo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Repositories.Abstracts
{
    public interface ICustomIdentityUserDAL
    {
        Task<List<CustomIdentityUser>> GetAllAsync();
        Task<CustomIdentityUser> GetByIdAsync(string id);
        Task AddAsync(CustomIdentityUser user);
        Task UpdateAsync(CustomIdentityUser user);
        Task DeleteAsync(CustomIdentityUser user);
    }
}
