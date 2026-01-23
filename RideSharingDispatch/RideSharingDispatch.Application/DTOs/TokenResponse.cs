using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.DTOs
{
    public class TokenResponse
    {
        public DateTime ExpiresAt;
        public string? Token;
    }
}
