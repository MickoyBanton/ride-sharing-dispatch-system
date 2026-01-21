using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterRider(Rider rider, User user);
        Task UnregisterRider(int userId);

        Task RegisterDriver(Driver driver, User user);
        Task UnregisterDriver(int userId);
    }
}
