namespace Booking.Core.Entities
{
    public class ApplicationUserGymClass
    {
        public string ApplicationUserId { get; set; }

        public int GymClassId { get; set; }


        //Nav prop

        public ApplicationUser? ApplicationUser { get; set; }

        public GymClass? GymClass { get; set; }



    }
}
