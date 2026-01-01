using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Infrastructure.Repositories
{
    public class DriverRepository: IDriverRepository
    {
        private readonly AppDbContext context;

        public DriverRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddDriver(Driver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            await context.Drivers.AddAsync(driver);
            await context.SaveChangesAsync();
        }

        public async Task RemoveDriver(Driver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            context.Drivers.Remove(driver);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ChangeDriverAvailability(bool isOnline, int userId)
        {
            var driver = await context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
            {
                return false;
            }

            driver.IsOnline = isOnline;
            await context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateDriverLocation(decimal latitude, decimal longitude, int userId)
        {
            var driver = await context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
            {
                return false;
            }

            driver.CurrentLatitude = latitude;
            driver.CurrentLongitude = longitude;
            await context.SaveChangesAsync();

            return true;

        }

        public async Task <Driver?> GetDriver(int userId)
        {
            return await context.Drivers.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<IEnumerable<Driver>> GetAllDrivers()
        {
            return await context.Drivers.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetOnlineDrivers()
        {
            return await context.Drivers.AsNoTracking().Where(d => d.IsOnline == true).ToListAsync();

        }

        public async Task<IEnumerable<Driver>> GetNearbyDrivers(decimal latitude, decimal longitude)
        {
            return await context.Drivers.ToListAsync(); //change
        }


    }
}
