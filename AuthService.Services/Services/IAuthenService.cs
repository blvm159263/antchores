using AuthService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services
{
	public interface IAuthenService
	{
        AuthenticateResponseModel Authenticate(string phoneNumber, string password);
	}
}