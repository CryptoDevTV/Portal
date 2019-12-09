namespace Portal.Academia.Web.ViewModels
{
    public abstract class BaseViewModel
    {
        public string UserGuid { get; set; }
        public string UserEmail { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public string Message { get; set; }
        public bool IsOk
            => string.IsNullOrWhiteSpace(Message);
    }
}