using System.Collections.Generic;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetContactsByTaskerId(int taskerId);


    }
}