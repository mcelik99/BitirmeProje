using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BitirmeProjesi.Data;

namespace BitirmeProjesi.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<User> SignInManager;
        private readonly BitirmeDBContext BitirmeDBContext;

        private IPasswordHasher<User> PasswordHasher;

        public LoginController(SignInManager<User> signInManager, BitirmeDBContext BitirmeDBContext, IPasswordHasher<User> passwordHasher)
        {
            this.SignInManager = signInManager;
            this.BitirmeDBContext = BitirmeDBContext;
            this.PasswordHasher = passwordHasher;    
        }

        [HttpGet]
        public IActionResult User()
        {
            return View(new UserLoginDto());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> User([FromForm] UserLoginDto Model)
        {
            if (ModelState.IsValid)
            {
                var Result = await this.SignInManager.PasswordSignInAsync(Model.Email, Model.Password, false, false);

                if (Result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Giriş İşlemi Başarısız");
                }
            }

            return View(Model);
        }


        public ActionResult Create()
        {
            User user = new User();

            user.Name = "Admin";
            user.Surname = "Çelik";
            user.PhoneNumber = "+905412723370";
            user.Email = "mehmetcelik.99@outlook.com";
        

            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.UserName = user.Email;

            user.NormalizedUserName = user.UserName.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            user.NormalizedEmail = user.Email.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            user.PasswordHash = this.PasswordHasher.HashPassword(user, "123456");

            user.SecurityStamp = Guid.NewGuid().ToString();

            this.BitirmeDBContext.Users.Add(user);
            this.BitirmeDBContext.SaveChanges();

            return View();
        }
    }
}