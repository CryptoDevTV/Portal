using Portal.Shared.Model.Entities;
using System.Collections.Generic;

namespace Portal.Academia.Web.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public CoursesViewModel()
        {
            Courses = new HashSet<Course>();
        }
        public IEnumerable<Course> Courses { get; set; }
    }
}