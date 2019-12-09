using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Academia.Web.ViewModels;
using Portal.Shared.Data;
using System.Threading.Tasks;

namespace Portal.Academia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataRepository _dataRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> Student(string userGuid)
        {
            var result = new CoursesViewModel();

            if (!string.IsNullOrWhiteSpace(userGuid))
            {
                var user = await _dataRepository.GetUserInfoAsync(userGuid);

                if(user is null)
                {
                    result.Message = $"Brak użytkownika dla {userGuid}!";
                    _logger.LogWarning(result.Message);
                }
                else
                {
                    result.Courses = await _dataRepository.GetAllCoursesByUserGuidAsync(userGuid);
                    result.UserGuid = userGuid;
                    result.UserEmail = user.Email;
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

        public IActionResult Policy()
            => View();
    }
}