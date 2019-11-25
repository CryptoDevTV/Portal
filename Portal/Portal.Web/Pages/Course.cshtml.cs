using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Portal.Web.Pages
{
    public class CourseModel : PageModel
    {
        private readonly ILogger<PageModel> _logger;

        public CourseModel(ILogger<PageModel> logger)
        {
            _logger = logger;
        }
        public string Name { get; set; }
        public int Id { get; set; }

        public void OnGet(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}