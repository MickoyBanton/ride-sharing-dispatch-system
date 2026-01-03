using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IFareCalculationService
    {
        Task<decimal> CalculateCost(decimal cost);
    }
}
