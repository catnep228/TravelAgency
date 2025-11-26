namespace TravelAgency.Models
{
    public class Order
    {
        public long Id { get; set; }
        public DateTime date {  get; set; }

        public List<Passanger> Passangers { get; set; } = [];

        public StatusOrder status {  get; set; }

        public List<Service>? AdditionalServices { get; set; } = [];

        public long tripId { get; set; }
        public Trip trip { get; set; }

    }
}
