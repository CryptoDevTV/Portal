using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Portal.Shared.Data;
using System.Net;
using System.Threading.Tasks;

namespace Portal.Web.Pages
{
    public class ConfirmationModel : PageModel
    {
        private readonly ILogger<ConfirmationModel> _logger;
        private readonly IDataRepository _dataRepository;

        public ConfirmationModel(
            ILogger<ConfirmationModel> logger,
            IDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        public string UserRegister { get; private set; }
        public string UserGuid { get; set; }

        public async Task OnGetRegister(string message)
        {
            var email = WebUtility.UrlDecode(message);

            var user = await _dataRepository.GetUserAsync(email);

            if (!(user is null))
            {
                UserRegister = user.Email;
                UserGuid = user.Guid;
            }
            else
            {
                _logger.LogError($"Error in confirmation for user: {email}.");
            }
        }
    }
}