using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using RideSharingDispatch.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context) 
        {

            this.context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
