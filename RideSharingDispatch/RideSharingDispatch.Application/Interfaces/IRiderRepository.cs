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
        void AddRider(Rider rider);
        void RemoveRider(Rider rider);
        Rider GetRider(int id);


    }
}
