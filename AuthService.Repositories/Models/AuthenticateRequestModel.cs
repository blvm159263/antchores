﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Models
{
    public class AuthenticateRequestModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
