namespace Portal.Shared.Model.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string ContentUrl { get; set; }
        public string ContentRawUrl { get; set; }
        public string Duration { get; set; }
    }
}