using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Academia.Web.ViewModels;
using Portal.Shared.Data;
using Portal.Shared.Model.Domain;
using Portal.Shared.Services.Notifications;
using Portal.Shared.Utils.Extensions;
using System.Threading.Tasks;

namespace Portal.Academia.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IEmailNotification _emailNotification;

        public ContactController(
            ILogger<ContactController> logger,
            IDataRepository dataRepository,
            IEmailNotification emailNotification)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _emailNotification = emailNotification;
        }

        public async Task<IActionResult> Index(string userGuid, int contactId)
        {
            var result = new ContactViewModel();

            if (!string.IsNullOrWhiteSpace(userGuid) || contactId < 0)
            {
                var user = await _dataRepository.GetUserInfoAsync(userGuid);

                if (user is null)
                {
                    result.Message = $"Brak użytkownika dla {userGuid}!";
                    _logger.LogWarning(result.Message);
                }
                else
                {
                    var ct = (ContactType)contactId;

                    result.UserGuid = userGuid;
                    result.UserEmail = user.Email;
                    result.Subject = ct.GetDescription();
                    result.ContactType = ct;
                    result.EmailContent = string.Empty;
                    _logger.LogInformation($"Dane dla {userGuid}");
                }
            }
            else
            {
                result.Message = "Proszę o podanie identyfikatora użytkownika!";
                _logger.LogWarning(result.Message);
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            var emailSent = await _emailNotification.SendAsync(
                $"[CryptoDev-Mail] {model.Subject}",
                $"{model.UserEmail} pisze: {model.EmailContent}",
                $"{model.UserEmail} pisze: {model.EmailContent}",
                new string[] { "tkowalczyk.poczta@gmail.com" }
                );

            if (emailSent.IsSuccess)
            {
                _logger.LogInformation($"{emailSent.Message}");

                return RedirectToAction("Student", "Home", new { userGuid = model.UserGuid });
            }
            else
            {
                _logger.LogError($"Error: {emailSent.Message} for user: {model.UserEmail}.");

                return View(new ContactViewModel { Message = emailSent.Message });
            }
        }
    }
}