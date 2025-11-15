namespace TravelAgency.Models
{
    public class User : Passanger

    {
        public string Email { get; set; }   
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Policy { get; set; }

        public Guid passangerId { get; set; }
        public Passanger passanger { get; set; }


        public Guid roleId { get; set; }
        public Role role { get; set; }
    }
}
