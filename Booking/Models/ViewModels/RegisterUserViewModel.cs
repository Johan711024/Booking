using Bogus.DataSets;
using Booking.Validations;
using System.ComponentModel.DataAnnotations;

namespace Booking.Models.ViewModels
{
    public class RegisterUserViewModel
    {

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]

        [FirstAndLastName]
        public string LastName { get; set; } = string.Empty;
    }
}
