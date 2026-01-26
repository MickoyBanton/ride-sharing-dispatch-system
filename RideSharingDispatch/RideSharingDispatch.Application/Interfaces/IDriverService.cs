using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IDriverService
    {
        Task<bool> UpdateLocation(decimal latitude, decimal longitude, int userId);
        Task<bool> SetOnlineStatus(bool isOnline, int userId);

    }
}
