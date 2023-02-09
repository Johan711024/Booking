
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Booking.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }

        [Display(Name = "Förnamn")]
        public string? FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string? LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        //Nav prop
        public ICollection<ApplicationUserGymClass>? ApplicationUserGymClasses { get; set; } = new List<ApplicationUserGymClass>();

    }
}
