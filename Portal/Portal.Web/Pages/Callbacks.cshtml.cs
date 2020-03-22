using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Portal.Web.ViewModels;

namespace Portal.Web.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class CallbacksModel : PageModel
    {
        private readonly ILogger<CallbacksModel> _logger;

        public CallbacksModel(
            ILogger<CallbacksModel> logger)
        {
            _logger = logger;
        }
        public void OnPost([FromBody]PaymentViewModel clb)
        {

        }
    }
}