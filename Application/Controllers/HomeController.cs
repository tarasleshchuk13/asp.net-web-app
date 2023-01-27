using Application.Data;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        IEnumerable<User> users = _db.User;
        return View(users);
    }

    public async Task<RedirectToActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }

    public async Task<RedirectToActionResult> Delete()
    {
        List<User> users = GetUserListFromForm();
        foreach (User user in users)
        {
            _db.User.Remove(user);
            _db.SaveChanges();
        }
        if (IsCurrentUserChecked(users))
        {
            await Logout();
        }
        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> Block()
    {
        List<User> users = GetUserListFromForm();
        foreach (User user in users)
        {
            user.IsBlocked = true;
            _db.SaveChanges();
        }
        if (IsCurrentUserChecked(users))
        {
            await Logout();
        }
        return RedirectToAction("Index");
    }

    public RedirectToActionResult Unblock()
    {
        List<User> users = GetUserListFromForm();
        foreach (User user in users)
        {
            user.IsBlocked = false;
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    private List<User> GetUserListFromForm()
    {
        var formData = HttpContext.Request.Form;
        List<User> users = new List<User>();
        foreach (var item in formData)
        {
            if (int.TryParse(item.Key, out int id))
            {
                User user = _db.User.Find(id);
                if (user != null)
                {
                    users.Add(user);
                }
            }
        }
        return users;
    }

    private bool IsCurrentUserChecked(List<User> users)
    {
        string userId = HttpContext.User.Identity.Name;
        foreach (User user in users)
        {
            if (user.Id.ToString() == userId)
            {
                return true;
            }
        }
        return false;
    }
}