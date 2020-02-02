using System.Collections.Generic;

namespace Portal.Shared.Model.Entities
{
    public class Course
    {
        public Course()
        {
            Modules = new HashSet<Module>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathMp3 { get; set; }
        public string PathMp4 { get; set; }
        public IEnumerable<Module> Modules { get; set; }
    }
}