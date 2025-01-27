using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.IDP.Pages.Account.ForgotPassword
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
