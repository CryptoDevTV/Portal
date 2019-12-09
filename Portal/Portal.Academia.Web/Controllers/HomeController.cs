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
                result.Courses = await _dataRepository.GetAllCoursesByUserGuidAsync(userGuid);
                result.UserGuid = userGuid;
                _logger.LogInformation($"Dane dla {userGuid}");
            }
            else
            {
                result.Message = "Proszę o podanie identyfikatora użytkownika!";
                _logger.LogWarning("Nie podano identyfikatora użytkownika");
            }

            return View(result);
        }

        public IActionResult Policy()
        {
            return View();
        }
    }
}