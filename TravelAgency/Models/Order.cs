namespace TravelAgency.Models;

public class Order
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public List<Passanger> Passangers { get; set; } = [];
    public StatusOrder Status { get; set; }     
    public List<Service>? AdditionalServices { get; set; } = [];
    public long TripId { get; set; }
    public Trip Trip { get; set; }
}
