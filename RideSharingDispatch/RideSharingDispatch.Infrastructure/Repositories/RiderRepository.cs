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
    public class RiderRepository: IRiderRepository
    {
        private readonly AppDbContext context; 
        public RiderRepository(AppDbContext context) 
        { 
            this.context = context; 
        }

        public async Task AddRiderAsync(Rider rider)
        {

            if (rider == null)
                throw new ArgumentNullException(nameof(rider));

            await context.Riders.AddAsync(rider);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRiderAsync(Rider rider)
        {
            if (rider == null)
                throw new ArgumentNullException(nameof(rider));

            context.Riders.Remove(rider);
            await context.SaveChangesAsync();
        }

        public async Task<Rider?> GetRiderAsync(int userId)
        {
            return await context.Riders.AsNoTracking().FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<IEnumerable<Rider>> GetAllRidersAsync()
        {
            return await context.Riders.AsNoTracking().ToListAsync();
        }

    }
}
