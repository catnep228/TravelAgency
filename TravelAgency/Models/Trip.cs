using Microsoft.AspNetCore.Antiforgery;

namespace TravelAgency.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress  { get; set; }
        public List<TypeOfHoliday> TypeOfHolidays { get; set; }
        public List<Service> Services { get; set; }
        public int NumberOfNights { get; set; }
        public List<Service> AdditionalService { get; set; }
        public DateTime date { get; set; }
        public float Price { get; set; }
        public StatusTrip status { get; set; }

    }
}
