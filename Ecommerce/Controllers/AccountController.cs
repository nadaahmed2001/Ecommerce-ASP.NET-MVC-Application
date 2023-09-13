using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyContext _dbContext;

        public AccountController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel loginModel = new LoginModel();
            loginModel.ReturnUrl = ReturnUrl;
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var users = _dbContext.Users;
            var user = _dbContext.Users.FirstOrDefault(u => u.userName == loginModel.userName && u.password == loginModel.Password);

            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("Home", "Code")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = loginModel.RememberMe
                    });

                return LocalRedirect(loginModel.ReturnUrl);
            }
            else
            {
                ViewBag.Message = "Invalid Credential";
                return View(loginModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if this user already exists in the database
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.userName == model.UserName || u.email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username or email already exists.");
                    return View(model);
                }

                var newUser = new User
                {
                    userName = model.UserName,
                    email = model.Email,
                    password = model.Password,
                    Age = model.Age,
                    Role = "User"
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                // Automatically log in the newly registered user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(newUser.Id)),
                    new Claim(ClaimTypes.Name, newUser.userName),
                    new Claim(ClaimTypes.Role, newUser.Role),
                    new Claim("Home", "Code")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false
                    });

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == Convert.ToInt32(userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateProfile(User user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != user.Id.ToString())
            {
                return Forbid();
            }

            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.userName = user.userName;
            existingUser.email = user.email;
            existingUser.Age = user.Age;

            if (!ModelState.IsValid)
            {
                return View("UserProfile", user);
            }

            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("UserProfile");
        }
    }
}
