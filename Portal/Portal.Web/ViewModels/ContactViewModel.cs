using Portal.Shared.Model.Domain;

namespace Portal.Web.ViewModels
{
    public class ContactViewModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public ContactType ContactType { get; set; }
    }
}