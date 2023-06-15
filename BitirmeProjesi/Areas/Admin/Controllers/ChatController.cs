using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitirmeProjesi.Services;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly BitirmeDBContext _context;
        private readonly MailService _mailService;

        public ChatController(BitirmeDBContext context, MailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public IActionResult Index(int id)
        {
            User CurrentUser = _context.Users.Where(x => x.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();

            Chat chat = this._context.Chats
                .Include("Student")
                .Include("User")
                .Include("ChatMessages")
                .Include("ChatMessages.User")
                .Include("ChatMessages.Student")
                .Where(x => x.UserId == CurrentUser.Id && x.StudentId == id)
                .FirstOrDefault();

            if (chat == null)
            {
                chat = new Chat();
                chat.CreateAt = DateTime.Now;
                chat.StudentId = id;
                chat.UserId = CurrentUser.Id;

                this._context.Chats.Add(chat);

                this._context.SaveChanges();

                return RedirectToAction("Create", new { id = id });
            }


            return View(chat);
        }

        [HttpPost]
        public IActionResult Create([Bind("ChatId,UserId,MessageBody")] ChatMessage chatMessage)
        {
            chatMessage.CreateAt = DateTime.Now;

            _context.Add(chatMessage);
            _context.SaveChanges();
           
            Chat chat = _context.Chats.FirstOrDefault(x => x.Id == chatMessage.ChatId);
            User user = _context.Users.FirstOrDefault(x => x.Id == chat.UserId);
            BitirmeProjesi.Models.Student student = _context.Students.FirstOrDefault(x => x.Id == chat.StudentId);


            string subject = "Yeni Mesaj";
            string body = $"Hocanız size yeni bir mesaj gönderdi: {chatMessage.MessageBody}";

            _mailService.SendEmail(student.Email, subject, body);

            return Redirect(Request.Headers["Referer"].ToString());
        }

    }

}
