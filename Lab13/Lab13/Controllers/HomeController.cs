using Microsoft.AspNetCore.Mvc;
using Lab13.Services;

namespace Lab13.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRandomNumberService _randomService;

        public HomeController(IMessageService messageService, IDateTimeService dateTimeService, IRandomNumberService randomService)
        {
            _messageService = messageService;
            _dateTimeService = dateTimeService;
            _randomService = randomService;
        }

        public IActionResult Index()
        {
            ViewBag.MessageFromService = _messageService.GetMessage();
            ViewBag.Now = _dateTimeService.GetNowFormatted();
            ViewBag.Random = _randomService.Next();
            return View();
        }
    }
}
