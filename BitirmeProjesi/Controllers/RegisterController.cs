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
        public IActionResult Student([FromForm] StudentRegisterDto Model)
        {
            if (ModelState.IsValid)
            {
                Student student = this._context.Students.Where(x => x.Email == Model.Email || x.StudentNumber == Model.Email).First();

                if (student == null)
                {
                    student = new Student();
                    student.Name = Model.Name;
                    student.Surname = Model.Surname;
                    student.StudentNumber = Model.StudentNumber;
                    student.Email = Model.Email;
                    student.Password = StudentPasswordHasher.Encrypt(Model.Password);
                    student.IsVerifed = false;
                    // TODO: Mail atılacak
                    this._context.Students.Add(student);
                    this._context.SaveChanges();


                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Daha Önce Alınmış Email veya Öğrenci Numarası");
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