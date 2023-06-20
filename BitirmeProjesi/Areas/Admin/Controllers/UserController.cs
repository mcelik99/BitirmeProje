using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using BitirmeProjesi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly BitirmeDBContext _context;
        private IPasswordHasher<User> PasswordHasher;
        private MailService _mailService;
        private GenerateRandomPassword _generateRandomPassword;

        public UserController(BitirmeDBContext context, IPasswordHasher<User> passwordHasher,MailService mailService,GenerateRandomPassword generateRandomPassword)
        {
            this.PasswordHasher = passwordHasher;
            _context = context;
            _mailService = mailService;
            _generateRandomPassword = generateRandomPassword;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            List<User> user = await _context.Users.ToListAsync();
            return View(user);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,Email,PhoneNumber,IsAdvisor,Quota")] User user)
        {         
            if (ModelState.IsValid)
            {
                
                user.UserName = user.Email;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.NormalizedUserName = user.UserName.ToUpperInvariant();
                user.NormalizedEmail = user.Email.ToUpperInvariant();
                // Rastgele şifre oluştur
                string randomPassword =_generateRandomPassword.GeneratePassword(8);
                // Kullanıcıya rastgele şifreyi e-posta ile gönder
                string subject = "Değerli Öğretim Görevlimiz  İçin Rastgele Şifre";
                string body = $" Değerli Öğretim Görevlimiz Yeni şifreniz: {randomPassword}";
                _mailService.SendEmail(user.Email, subject, body);
                string hashedPassword=this.PasswordHasher.HashPassword(user , randomPassword);
                user.PasswordHash = hashedPassword;
                user.SecurityStamp = Guid.NewGuid().ToString();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuotaForAllUsers(int quota)
        {
            if (ModelState.IsValid)
            {
                List<User> users = await _context.Users.ToListAsync();
                foreach (User user in users)
                {
                    user.Quota = quota;
                    _context.Entry(user).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
