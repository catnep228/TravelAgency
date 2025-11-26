namespace TravelAgency.Models
{
    public class TypeOfHoliday
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeActivity TypeOfACt { get; set; }

        public List<Trip>? Trips { get; set; } = [];
    }
}
