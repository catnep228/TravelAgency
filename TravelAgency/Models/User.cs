namespace TravelAgency.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }

        public long? PassangerId { get; set; }
        public Passanger Passanger { get; set; }

        public long? RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
