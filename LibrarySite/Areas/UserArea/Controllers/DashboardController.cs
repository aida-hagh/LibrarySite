using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
