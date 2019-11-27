using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Portal.Shared.Services.Notifications;
using Portal.Web.ViewModels;

namespace Portal.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;
        private readonly IEmailNotification _emailNotification;

        public ContactModel(
            ILogger<ContactModel> logger,
            IEmailNotification emailNotification)
        {
            _logger = logger;
            _emailNotification = emailNotification;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public ContactViewModel ContactViewModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var emailSent = await _emailNotification.SendAsync(
                $"[CryptoDev-Mail] {ContactViewModel.Subject}",
                $"{ContactViewModel.Email} pisze: {ContactViewModel.Message}",
                $"{ContactViewModel.Email} pisze: {ContactViewModel.Message}",
                new string[] { "tkowalczyk.poczta@gmail.com" }
                );

            if (emailSent.IsSuccess)
            {
                _logger.LogInformation($"{emailSent.Message}");

                return RedirectToPage("/ThankYou", "MailSent");
            }
            else
            {
                _logger.LogError($"Error: {emailSent.Message} for user: {ContactViewModel.Email}.");

                return RedirectToPage("/Error", "UserError", new { message = emailSent.Message });
            }
        }
    }
}