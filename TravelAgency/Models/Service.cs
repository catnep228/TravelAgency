namespace TravelAgency.Models
{
    public class Service
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TypeService { get; set; }
        public List<Order>? Orders { get; set; } = [];
        public List<Trip>? Trips { get; set; } = [];
        public List<Trip>? TripsAdditional { get; set; } = [];

    }
}
