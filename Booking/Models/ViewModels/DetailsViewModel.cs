namespace Booking.Models.ViewModels
{
    public class DetailsViewModel
    {
        public int GymClassId { get; set; }

        public string? GymClassName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTim { get { return StartTime + Duration; } }
        public string? Description { get; set; }

        public List<ApplicationUserGymClass>? Attendants { get; set; }
    }
}
