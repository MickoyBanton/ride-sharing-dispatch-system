using RideSharingDispatch.Application.Interfaces;
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

        public Task<UserRole> GetUserRole(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserValid(int userId, string password)
        {
            throw new NotImplementedException();
        }
    }
}
