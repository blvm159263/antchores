namespace AuthService.BusinessObjects.Models
{
    public class CustomerReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool Status { get; set; }

        public int AccountId { get; set; }
    }
}