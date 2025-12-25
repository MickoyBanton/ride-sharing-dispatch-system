using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Domain.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        public int RiderId { get; set; }
        public int? DriverId { get; set; }
        public decimal PickupLatitude { get; set; }
        public decimal PickupLongitude { get; set; }
        public decimal DestinationLatitude { get; set; }
        public decimal DestinationLongitude { get; set; }

        public decimal Fare { get; set; }
        public VehicleType VehicleType { get; set; }

        public TripStatus TripStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }


    }
}
