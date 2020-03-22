using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Web.ViewModels;

namespace Portal.Web.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ShopModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPost([FromBody]PaymentViewModel clb)
        {

        }
    }
}