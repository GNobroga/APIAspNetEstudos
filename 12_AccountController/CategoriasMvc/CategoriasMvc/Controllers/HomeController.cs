using Microsoft.AspNetCore.Mvc;

namespace CategoriasMvc.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}