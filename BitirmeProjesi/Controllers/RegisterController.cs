using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.Data;
using BitirmeProjesi.Services;
using System.Net.Mail;
using System.Net;

namespace BitirmeProjesi.Controllers
{
    public class RegisterController : Controller
    {

        private readonly BitirmeDBContext _context;
        private readonly MailService _mailService;

        public RegisterController(BitirmeDBContext context, MailService mailService)
        {
            this._context = context;
            _mailService = mailService; 
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
                Student student = this._context
                    .Students.Where(x => x.Email == Model.Email || x.StudentNumber == Model.StudentNumber)
                    .FirstOrDefault();

                if (student == null)
                {
                    student = new Student();
                    student.Name = Model.Name;
                    student.Surname = Model.Surname;
                    student.StudentNumber = Model.StudentNumber;
                    student.Email = Model.Email;
                    student.Password = StudentPasswordHasher.Encrypt(Model.Password);
                    student.IsVerifed = false;
                    student.VerifedCode = Model.Name.Substring(0, 2) + DateTime.Today.Year;
                    student.CreateAt = DateTime.Now;
                    // TODO: Mail atılacak
                    this._context.Students.Add(student);
                    this._context.SaveChanges();

                    string emailAddress = student.Email;
                    string subject = "Hesap Doğrulama Kodu";
                    string body = $"Hesap doğrulama kodunuz: {student.VerifedCode}";

                    _mailService.SendEmail(emailAddress, subject, body);

                    ViewData["SuccessMessage"] = "Kayıt İşlemi Başarılı Lütfen Mailinizi Kontrol Ediniz !";

                    Model = new StudentRegisterDto();

                    return RedirectToAction("Verify");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Daha Önce Alınmış Email veya Öğrenci Numarası");
                }
            }

            return View(Model);
        }
        [HttpGet]
        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ViewData["ErrorMessage"] = "Geçersiz doğrulama kodu.";
                return View();
            }

            Student student = this._context.Students.FirstOrDefault(x => x.VerifedCode == code);

            if (student != null)
            {
                student.IsVerifed = true;
                this._context.SaveChanges();

                ViewData["SuccessMessage"] = "Hesabınız başarıyla doğrulandı!";
            }
            else
            {
                ViewData["ErrorMessage"] = "Geçersiz doğrulama kodu.";
            }

            return View();
        }

       
    }
}