using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portal.Web.Pages
{
    public class ThankYouModel : PageModel
    {
        public string Message { get; private set; }
        public void OnGet()
        {
        }

        public void OnGetMailSent()
        {
            Message = "Wysłano wiadomość, odpowiem tak szybko jak to możliwe. Dziękuje!";
        }
    }
}