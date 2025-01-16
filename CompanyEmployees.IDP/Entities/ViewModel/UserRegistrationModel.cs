using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.IDP.Entities.ViewModel
{
    public class UserRegistrationModel
    {
        public string FirstName { get; set; } 
        public string Lastame { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not nmatch.")]
        public string ConfirmPassword { get; set; }
    }
}
