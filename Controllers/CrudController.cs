using Microsoft.AspNetCore.Mvc;

namespace UserAdminPortal.Controllers
{
    public class CrudController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
