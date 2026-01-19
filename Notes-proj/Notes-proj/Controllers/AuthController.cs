using Microsoft.AspNetCore.Mvc;
using Notes_proj.Models;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            return RedirectToAction("Index", "Notes");
        }
        ViewBag.Error = "The login information is incorrect";
        return View();
    }

    
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (_context.Users.Any(u => u.Username == username))
        {
            ViewBag.Error = "Username already exists";
            return View();
        }

        var user = new User { Username = username, Password = password };
        _context.Users.Add(user);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", user.Id);
        return RedirectToAction("Index", "Notes");
    }

    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    

}
