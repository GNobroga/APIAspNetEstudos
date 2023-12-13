using Microsoft.AspNetCore.Mvc;

namespace test.Controllers;

public class AppController : Controller
{
    public IActionResult Index() 
    {
        return View(new string[] { "Gabriel", "Pedro", "Maria" });
    }
}