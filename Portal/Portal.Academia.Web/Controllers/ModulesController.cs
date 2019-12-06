using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Academia.Web.ViewModels;
using Portal.Shared.Data;

namespace Portal.Academia.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly IDataRepository _dataRepository;

        public ModulesController(
            ILogger<ModulesController> logger,
            IDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }
        public async Task<IActionResult> Show(string userGuid, int courseId)
        {
            var result = new ModulesViewModel();

            if(!string.IsNullOrWhiteSpace(userGuid) && courseId > 0)
            {
                result.UserGuid = userGuid;
                result.CourseId = courseId;

                result.Modules = await _dataRepository.GetAllModulesByUserGuidAndModuleIdAsync(userGuid, courseId);
            }
            else
            {
                result.Message = "Proszę o podanie identyfikatora użytkownika i kursu!";
            }

            return View(result);
        }
    }
}