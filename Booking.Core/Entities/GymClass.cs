namespace Booking.Core.Entities
{
    public class GymClass
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTim { get { return StartTime + Duration; } }
        public string? Description { get; set; }
        public DateTime Created { get; set; }

        //Nav prop
        public ICollection<ApplicationUserGymClass>? ApplicationUserGymClasses { get; set; } = new List<ApplicationUserGymClass>();
    }
}
