using CompanyEmployees.IDP.Entities;
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEmployees.IDP.Pages.Account.ForgotPassword
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ForgotPasswordModel forgotPassword { get; set; }

        public IndexModel(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult OnGet(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost(string returnUrl)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation", "ForgotPasswordConfirmation");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPasswordRequest), "Account", new { token, email = user.Email, returnUrl }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);

            return RedirectToAction("ForgotPasswordConfirmation", "ForgotPasswordConfirmation");
        }
    }
        

}
