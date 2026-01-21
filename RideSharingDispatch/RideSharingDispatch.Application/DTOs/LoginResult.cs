using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.DTOs
{
    public class LoginResult
    {
        public bool IsSuccessful { get; set; }
        public int? UserId { get; set; }
        public UserRole? Role { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
