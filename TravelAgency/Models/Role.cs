namespace TravelAgency.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<User>? Users { get; set; } = [];
    }
}
