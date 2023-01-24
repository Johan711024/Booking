using Microsoft.AspNetCore.Identity;

namespace Booking.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }

        //Nav prop
        public ICollection<ApplicationUserGymClass>? ApplicationUserGymClasses { get; set; } = new List<ApplicationUserGymClass>();

    }
}
