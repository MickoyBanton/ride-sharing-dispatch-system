using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IRiderRepository
    {
        Task AddRiderAsync(Rider rider);
        Task RemoveRiderAsync(Rider rider);
        Task <Rider?> GetRiderAsync(int userId);
        Task<IEnumerable<Rider>> GetAllRidersAsync();
    }
}
