﻿using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int id);
    }
}
