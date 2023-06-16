using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.Models;
using BitirmeProjesi.Services;
using BitirmeProjesi.Data;
using Microsoft.AspNetCore.Identity;

namespace BitirmeProjesi.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly BitirmeDBContext _context;
        private readonly MailService _mailService;
        private GenerateRandomPassword _generateRandomPassword;

        public ResetPasswordController(BitirmeDBContext context, MailService mailService,GenerateRandomPassword generateRandomPassword)
        {
            _context = context;
            _mailService = mailService;
            _generateRandomPassword = generateRandomPassword;
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);

            if (student == null)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresine sahip bir öğrenci bulunamadı.");
                return View();
            }
           
            string newPassword = _generateRandomPassword.GeneratePassword(8);
           
            string subject = "Öğrenci için Yeni Şifre";
            string body = $"Değerli Öğrencimiz Yeni şifreniz: {newPassword}";
            _mailService.SendEmail(student.Email, subject, body);
           
            string hashedPassword = StudentPasswordHasher.Encrypt(newPassword);
            student.Password = hashedPassword;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Şifreniz sıfırlandı. Yeni şifreniz e-posta adresinize gönderildi.";

            return View();
        }

    }
}