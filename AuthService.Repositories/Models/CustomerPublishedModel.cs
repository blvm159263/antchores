namespace AuthService.Repositories.Models
{
    public class CustomerPublishedModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Event { get; set; }

        public bool Status { get; set; }
    }
}