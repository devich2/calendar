using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Calendar.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}