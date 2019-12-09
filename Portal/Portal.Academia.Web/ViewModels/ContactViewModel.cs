using Portal.Shared.Model.Domain;

namespace Portal.Academia.Web.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public string Subject { get; set; }
        public string EmailContent { get; set; }
        public ContactType ContactType { get; set; }
    }
}