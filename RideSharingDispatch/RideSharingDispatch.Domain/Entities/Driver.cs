using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Domain.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsOnline { get; set; }

        public decimal CurrentLatitude { get; set; }
        public decimal CurrentLongitude { get; set; }

        public VehicleType VehicleType { get; set; }

    }
}
