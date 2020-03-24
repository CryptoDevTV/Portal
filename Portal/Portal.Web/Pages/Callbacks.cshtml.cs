using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Portal.Shared.Data;
using Portal.Shared.Model.Entities;
using Portal.Shared.Services.Notifications;
using Portal.Web.Extensions;
using Portal.Web.ViewModels;
using System.Threading.Tasks;

namespace Portal.Web.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class CallbacksModel : PageModel
    {
        private readonly ILogger<CallbacksModel> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IEmailNotification _emailNotification;
        private readonly IPushoverNotification _pushoverNotification;

        public CallbacksModel(
            ILogger<CallbacksModel> logger,
            IDataRepository dataRepository,
            IEmailNotification emailNotification,
            IPushoverNotification pushoverNotification)
        {
            _logger = logger;
            _dataRepository = dataRepository;
            _emailNotification = emailNotification;
            _pushoverNotification = pushoverNotification;
        }


        public async Task OnPost()
        {
            var callback = await RequestExt.GetObject<PaymentViewModel>(Request.Body);

            if (!(callback is null))
            {
                var added = await _dataRepository.CreateOrderHistoryAsync(
                    new Order
                    {
                        OrderMnemonic = callback.Data.Id,
                        Status = callback.Data.Status,
                        Email = callback.Data.BuyerFields.BuyerEmail,
                        BtcPrice = callback.Data.BtcPaid,
                        Rate = callback.Data.Rate.ToString()
                    });

                if(added == 1)
                {
                    var msg = $"Order {callback.Data.Id} ({callback.Data.Status}) added for {callback.Data.BuyerFields.BuyerEmail}";

                    _logger.LogInformation(msg);

                    _pushoverNotification.Send("GDI Order (new)", $"{msg}");

                    if(callback.Data.Status == "paid")
                    {
                        var user = await _dataRepository.GetUserAsync(callback.Data.BuyerFields.BuyerEmail);

                        if(!(user is null))
                        {
                            var addUserToCourse = await _dataRepository.AddUserToCourseAsync(
                                new UserCourse
                                {
                                    UserId = user.UserId,
                                    CourseId = 1 // GDI only now
                                });

                            _pushoverNotification.Send("GDI Order (paid)", $"{msg}");

                            if (addUserToCourse == 1)
                            {
                                var link = $"https://app.cryptodev.tv/student/{user.Guid}";

                                var email = await _emailNotification.SendAsync(
                                    "GitHub dla Inwestorów już dostępny dla Ciebie",
                                    $"Dziękuje za zakup. Kurs dostępny tutaj: {link}",
                                    $"Dziękuje za zakup. Kurs dostępny <a href='{link}'>tutaj</a>",
                                    new string[] { user.Email });

                                if(email.IsSuccess)
                                {
                                    _pushoverNotification.Send("GDI Order (sent)", $"{msg}");
                                }
                                else
                                {
                                    _logger.LogWarning($"{email.Message}");

                                    _pushoverNotification.Send("GDI Error (mail)", $"{email.Message}");
                                }
                            }
                        }
                        else
                        {
                            var err1 = $"User {callback.Data.BuyerFields.BuyerEmail} not assigned for course";

                            _logger.LogWarning($"{err1}");

                            _pushoverNotification.Send("GDI Error (assigned)", $"{err1}");
                        }
                    }
                }
                else
                {
                    var err2 = $"Order not added for {callback.Data.BuyerFields.BuyerEmail}";

                    _logger.LogWarning(err2);

                    _pushoverNotification.Send("GDI Error (not added)", $"{err2}");
                }
            }
            else
                _logger.LogWarning("Empty callback body");
        }
    }
}