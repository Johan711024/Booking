using System.ComponentModel.DataAnnotations;

namespace Booking.Validations
{
    public class FirstAndLastName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var ErrorMessage = "First and Last Name must be different";

            //if (value is string input)
            //{
            //    //var input = value as string;
            //    var viewModel = validationContext.ObjectInstance as OwnerCreateViewModel;

            //    if (viewModel is not null)
            //    {
            //        if (viewModel.FirstName != input)
            //        {
            //            return ValidationResult.Success;
            //        }
            //    }
            //}

            return new ValidationResult(ErrorMessage);

        }
    }
}
