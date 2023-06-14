using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChatController : BaseController
    {
        private readonly BitirmeDBContext _context;

        public ChatController(BitirmeDBContext context)
        {

            _context = context;
        }

        public IActionResult Index(int id)
        {
           


            Chat chat = this._context.Chats
                .Include("Student")
                .Include("User")
                .Include("ChatMessages")
                .Include("ChatMessages.User")
                .Include("ChatMessages.Student")
                .Where(x => x.UserId == CurrentUser.Id && x.StudentId == id)
                .FirstOrDefault();

            return View(chat);
        }

        [HttpPost]
        public IActionResult Create([Bind("ChatId,UserId,MessageBody")] ChatMessage chatMessage)
        {
            chatMessage.CreateAt = DateTime.Now;

            _context.Add(chatMessage);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

}
