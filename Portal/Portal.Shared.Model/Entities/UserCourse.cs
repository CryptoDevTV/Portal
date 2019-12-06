namespace Portal.Shared.Model.Entities
{
    public class UserCourse
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public bool IsPurchased { get; set; }
    }
}