namespace TravelAgency.Models
{
    public class User

    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public string InternationalPassport { get; set; }
        public string Policy { get; set; }


    }
}
