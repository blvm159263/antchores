using System;
using AuthService.Repositories.Enums;

namespace AuthService.Repositories.Models
{
    public class AccountReadModel{
        public int Id { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }

    }
}