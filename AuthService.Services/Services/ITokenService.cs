using AuthService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services
{
    public interface ITokenService
    {
        string GenerateToken(AccountModel accountModel);
    }
}
