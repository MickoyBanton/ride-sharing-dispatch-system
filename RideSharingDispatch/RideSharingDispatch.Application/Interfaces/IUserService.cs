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
        Task RegisterRider(Rider rider);
        Task UnregisterRider(Rider rider);

        Task RegisterDriver(Driver driver);
        Task UnregisterDriver(Driver driver);
    }
}
