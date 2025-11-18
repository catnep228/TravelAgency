namespace TravelAgency.Models
{
    public class User

    {
        public long Id { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }

        public long passangerId { get; set; }
        public Passanger passanger { get; set; }


        public long roleId { get; set; }
        public Role role { get; set; }
    }
}
