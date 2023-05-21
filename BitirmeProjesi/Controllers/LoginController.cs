using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BitirmeProjesi.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using BitirmeProjesi.Services;

namespace BitirmeProjesi.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;
        private readonly BitirmeDBContext BitirmeDBContext;

        private IPasswordHasher<User> PasswordHasher;

        public LoginController(
            SignInManager<User> signInManager,
            BitirmeDBContext BitirmeDBContext,
            IPasswordHasher<User> passwordHasher,
            UserManager<User> userManager
            )
        {
            this.SignInManager = signInManager;
            this.BitirmeDBContext = BitirmeDBContext;
            this.PasswordHasher = passwordHasher;
            this.UserManager = userManager;
        }


        [HttpGet]
        public IActionResult Student()
        {
            return View(new StudentLoginDto());
        }

        [HttpPost]
        public IActionResult Academician([FromForm] StudentLoginDto Model)
        {
            if (ModelState.IsValid)
            {
                Student student = this.BitirmeDBContext.Students.Where(x => x.Email == "").First();

                if (student != null && student.Password == StudentPasswordHasher.Encrypt(Model.Password))
                {
                    this.HttpContext.Session.SetInt32("STUDENT_ID", student.Id);
                    this.HttpContext.Session.SetString("STUDENT_FULL_NAME", student.FullName());

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Giriş İşlemi Başarısız");
                }
            }

            return View(Model);
        }


        [HttpGet]
        public IActionResult Academician()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View(new UserLoginDto());
        }

        [HttpPost]

        public async System.Threading.Tasks.Task<IActionResult> Academician([FromForm] UserLoginDto Model)
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

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> Out()
        {
            await this.SignInManager.SignOutAsync();
            return RedirectToAction("Academician", "Login");
        }


        public ActionResult Create()
        {
            User user = new User();

            user.Name = "Admin";
            user.Surname = "Çelik";
            user.PhoneNumber = "+905412723370";
            user.Email = "mehmetcelik99@outlook.com";
            user.UserName = "mehmetcelik99@outlook.com";


            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;

            user.NormalizedUserName = user.UserName.ToUpper();
            user.NormalizedEmail = user.Email.ToUpper();
            user.PasswordHash = this.PasswordHasher.HashPassword(user, "Test1234");

            user.SecurityStamp = Guid.NewGuid().ToString();

            this.BitirmeDBContext.Users.Add(user);
            this.BitirmeDBContext.SaveChanges();
            this.BitirmeDBContext.Database.CloseConnection();

            return View();
        }
    }
}