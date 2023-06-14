using Azure.Core;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly BitirmeDBContext _context;

        public ChatController(BitirmeDBContext context)
        {

            _context = context;
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

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

}
