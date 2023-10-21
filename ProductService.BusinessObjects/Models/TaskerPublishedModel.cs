namespace ProductService.BusinessObjects.Models
{
    public class TaskerPublishedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Identification { get; set; }
        public string Event { get; set; }
        public bool Status { get; set; }
    }
}