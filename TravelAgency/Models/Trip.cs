using Microsoft.AspNetCore.Antiforgery;
using System.Net;

namespace TravelAgency.Models
{
    public class Trip
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress  { get; set; }


        public Guid typeofHoldayId { get; set; }
        public TypeOfHoliday typeOfHoliday { get; set; }


        public List<Service> Services { get; set; } = [];
        public List<Service> AdditionalServices { get; set; } = [];
        public int NumberOfNights { get; set; }


        public DateTime date { get; set; }
        public float Price { get; set; }
        public StatusTrip status { get; set; }

        public List<Order> Orders { get; set; }

    }
}
