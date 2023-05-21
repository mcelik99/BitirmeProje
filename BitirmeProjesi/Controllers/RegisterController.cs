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
    public class RegisterController : Controller
    {

        private readonly BitirmeDBContext _context;

        public RegisterController(BitirmeDBContext context)
        {
            this._context = context;
        }


        [HttpGet]
        public IActionResult Student()
        {
            return View(new StudentRegisterDto());
        }

        [HttpPost]
        public IActionResult Academician([FromForm] StudentRegisterDto Model)
        {
            if (ModelState.IsValid)
            {
                Student student = this._context.Students.Where(x => x.Email == Model.Email || x.StudentNumber == Model.Email).First();

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
            //user.PasswordHash = this.PasswordHasher.HashPassword(user, "Test1234");

            user.SecurityStamp = Guid.NewGuid().ToString();

            this._context.Users.Add(user);
            this._context.SaveChanges();
            this._context.Database.CloseConnection();

            return View();
        }
    }
}