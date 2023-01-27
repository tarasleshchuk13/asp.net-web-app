using System.Security.Claims;
using Application.Data;
using Application.Models;
using Application.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _db;
    
    public AuthController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Login()
    {
        return View();
    }

    private void UpdateLastLoginDateForUser(User user)
    {
        user.LastLoginDate = DateTime.Now;
        _db.SaveChanges();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        User user = GetUserFromDbByEmail(loginVM.Email);
        if (user == null || user.Password != loginVM.Password)
        {
            ViewBag.ErrorMessage = "Wrong email or password";
            return View();
        }
        if (user.IsBlocked)
        {
            ViewBag.ErrorMessage = "This user is blocked";
            return View();
        }
        UpdateLastLoginDateForUser(user);
        SetCookie(user.Id.ToString());
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        User userFromDb = GetUserFromDbByEmail(registerVM.Email);
        if (userFromDb != null)
        {
            ViewBag.ErrorMessage = "User with this email already exist";
            return View();
        }
        User user = new User()
        {
            Email = registerVM.Email,
            Name = registerVM.Name,
            Password = registerVM.Password,
        };
        _db.User.Add(user);
        _db.SaveChanges();
        SetCookie(user.Id.ToString());
        return RedirectToAction("Index", "Home");
    }

    private User GetUserFromDbByEmail(string email)
    {
        return _db.User.FirstOrDefault(u => u.Email == email);
    }
    async private void SetCookie(string userId)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userId) };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }
}