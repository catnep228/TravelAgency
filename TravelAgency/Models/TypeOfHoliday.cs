namespace TravelAgency.Models
{
    public class TypeOfHoliday
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TypeActivity TypeOfACt { get; set; }

        public List<Trip> Trips { get; set; } = [];
    }
}
