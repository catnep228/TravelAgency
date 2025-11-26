namespace TravelAgency.Models
{
    public class Passanger
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool BirthCertificate { get; set; }
        public string Passport { get; set; }
        public string InternationalPassport { get; set; }
        public string Policy { get; set; }
        public List<Order> Orders { get; set; } = [];

        public long? userId { get; set; }
        public User? user { get; set; }

    }
}
