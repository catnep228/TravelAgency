namespace TravelAgency.Models
{
    public class Order
    {
        public int Id { get; set; }
        DateTime date {  get; set; }
        List<Passanger> passangers { get; set; }
        public StatusOrder status {  get; set; }
        public List<Service> AdditionalService { get; set; }
        public Trip trip { get; set; }

    }
}
