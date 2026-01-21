using Microsoft.AspNetCore.Identity;
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

        public async Task AddRiderAsync(Rider rider, User user)
        {

            if (rider == null || user == null)
                throw new ArgumentNullException(nameof(rider));

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            rider.UserId = user.Id;


            await context.Riders.AddAsync(rider);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRiderAsync(int userId)
        {
            var rider = await context.Riders.FindAsync(userId);
            var user = await context.Users.FindAsync(userId);

            if (rider == null)
                throw new ArgumentException("Rider not found", nameof(userId));

            if (user == null)
                throw new ArgumentException("User not found", nameof(userId));

            context.Riders.Remove(rider);
            context.Users.Remove(user);

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
