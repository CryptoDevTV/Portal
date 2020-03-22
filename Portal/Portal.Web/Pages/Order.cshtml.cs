using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Portal.Shared.Data;
using Portal.Shared.Model.Entities;
using Portal.Web.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace Portal.Web.Pages
{
    public class OrderModel : PageModel
    {
        private readonly ILogger<OrderModel> _logger;
        private readonly IDataRepository _dataRepository;

        public OrderModel(
            ILogger<OrderModel> logger,
            IDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var exist = await _dataRepository.GetUserAsync(OrderViewModel.Email);

            if (!(exist is null))
            {
                var msg = $"User already exist: {OrderViewModel.Email}.";

                _logger.LogError(msg);

                return RedirectToPage("/Error", "UserError", new { message = msg });
            }

            var created = await _dataRepository.CreateUserAsync(new User { Email = OrderViewModel.Email });

            if (created == 1)
            {
                _logger.LogInformation($"register user {OrderViewModel.Email}");

                return RedirectToPage("/confirmation", "Register", new { message = WebUtility.UrlEncode(OrderViewModel.Email) });
            }
            else
            {
                var msg = $"Error with order for user: {OrderViewModel.Email}.";

                _logger.LogError(msg);

                return RedirectToPage("/Error", "UserError", new { message = msg });
            }
        }
    }
}