using Microsoft.AspNetCore.Mvc;
using UserAdminPortal.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using UserAdminPortal.Data;

public class BaseController : Controller
{
    private readonly AppDbContext _context;

    public BaseController(AppDbContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Fetch the logged-in admin
        var loggedInAdmin = _context.Admins.FirstOrDefault(a => a.UserName == User.Identity.Name);

        if (loggedInAdmin != null)
        {
            ViewBag.AdminUserName = loggedInAdmin.UserName;
        }

        base.OnActionExecuting(context);
    }
}
