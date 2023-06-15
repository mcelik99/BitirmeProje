using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Student")]
    public class ChatController : BaseController
    {
        private readonly BitirmeDBContext _context;

        public ChatController(BitirmeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            int StudentId = this.getStudentID();
            List<Chat> chats = this._context.Chats
                 .Include("Student")
                 .Include("User")
                 .Include("ChatMessages")
                 .Include("ChatMessages.User")
                 .Include("ChatMessages.Student")
                 .Where(x => x.StudentId == StudentId)
                 .ToList();

            return View(chats);
        }


        public IActionResult Details(int id)
        {
            Chat chat = this._context.Chats
                .Include("Student")
                .Include("User")
                .Include("ChatMessages")
                .Include("ChatMessages.User")
                .Include("ChatMessages.Student")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return View(chat);
        }

        [HttpPost]
        public IActionResult Create([Bind("ChatId,StudentId,MessageBody")] ChatMessage chatMessage)
        {
            chatMessage.CreateAt = DateTime.Now;

            _context.Add(chatMessage);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

}
