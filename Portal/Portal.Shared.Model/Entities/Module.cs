using System.Collections.Generic;

namespace Portal.Shared.Model.Entities
{
    public class Module
    {
        public Module()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Lesson> Lessons { get; set; }
    }
}