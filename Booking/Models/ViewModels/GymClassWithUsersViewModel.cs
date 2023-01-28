namespace Booking.Models.ViewModels
{
    public class GymClassWithUsersViewModel
    {
        public string? ApplicationUserId { get; set; }

        public bool attending { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public int GymClassId { get; set; }


        public string? GymClassName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTim { get { return StartTime + Duration; } }
        public string? Description { get; set; }



    }
}
