using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.ViewModels;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IServiceOrderService _serviceOrderService;

        public HomeController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _serviceOrderService.GetAllAsync();
            var activeOrdersCount = allOrders.Count(o =>
                o.Status == Models.ServiceOrderStatus.Pending ||
                o.Status == Models.ServiceOrderStatus.InProgress);
            var completedOrdersCount = allOrders.Count(o =>
                o.Status == Models.ServiceOrderStatus.Completed);

            var model = new HomeIndexViewModel
            {
                ActiveOrdersCount = activeOrdersCount,
                CompletedOrdersCount = completedOrdersCount,
                WelcomeMessage = "Witamy w aplikacji serwisowej!"
            };

            return View(model);
        }

        [AllowAnonymous]
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // *** DODANA AKCJA PRIVACY ***
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}