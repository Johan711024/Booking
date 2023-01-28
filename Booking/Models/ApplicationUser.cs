using Microsoft.AspNetCore.Identity;

namespace Booking.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        //Nav prop
        public ICollection<ApplicationUserGymClass>? ApplicationUserGymClasses { get; set; } = new List<ApplicationUserGymClass>();

    }
}
