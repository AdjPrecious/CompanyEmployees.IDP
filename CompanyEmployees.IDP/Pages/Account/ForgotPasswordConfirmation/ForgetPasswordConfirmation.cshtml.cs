using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEmployees.IDP.Pages.Account.ForgotPasswordConfirmation
{
    public class ForgetPasswordConfirmationModel : PageModel
    {


        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
