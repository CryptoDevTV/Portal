using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Academia.Web.ViewModels;
using Portal.Shared.Data;
using System.Threading.Tasks;

namespace Portal.Academia.Web.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ILogger<LessonsController> _logger;
        private readonly IDataRepository _dataRepository;

        public LessonsController(
            ILogger<LessonsController> logger,
            IDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> Index(string userGuid, int courseId, int lessonId)
        {
            var result = new LessonsViewModel();

            if (!string.IsNullOrWhiteSpace(userGuid) && lessonId > 0)
            {
                result.UserGuid = userGuid;
                result.LessonId = lessonId;

                var lesson = await _dataRepository.GetLessonByUserGuidAndLessonId(userGuid, lessonId);

                if (!(lesson is null))
                {
                    result.ContentUrl = lesson.ContentUrl;
                    result.Name = lesson.Name;

                    result.CourseId = courseId;
                }
            }
            else
            {
                result.Message = "Proszę o podanie identyfikatora użytkownika i kursu!";
            }

            return View(result);
        }
    }
}