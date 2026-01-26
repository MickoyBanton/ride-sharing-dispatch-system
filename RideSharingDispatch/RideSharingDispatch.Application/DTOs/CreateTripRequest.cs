using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.DTOs
{
    public class CreateTripRequest
    {
        public int RiderId { get; set; }

        public decimal PickupLatitude { get; set; }
        public decimal PickupLongitude { get; set; }

        public decimal DestinationLatitude { get; set; }
        public decimal DestinationLongitude { get; set; }

        public VehicleType VehicleType { get; set; }
    }

}
