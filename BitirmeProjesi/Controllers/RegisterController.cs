using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.Data;
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

                    ViewData["SuccessMessage"] = "Kayıt İşlemi Başarılı Lütfen Mailinizi Kontrol Ediniz !";

                    Model = new StudentRegisterDto();


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Daha Önce Alınmış Email veya Öğrenci Numarası");
                }
            }

            return View(Model);
        }
    }
}