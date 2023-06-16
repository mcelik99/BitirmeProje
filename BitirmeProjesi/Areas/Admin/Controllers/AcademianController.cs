using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AcademianController : Controller
    {
        private readonly BitirmeDBContext _context;

        public AcademianController(BitirmeDBContext context)
        {

            _context = context;
        }

        public IActionResult Index()
        {
            User CurrentUser = _context.Users.Where(x => x.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();


            string Sql = "Select Periods.* from Periods " +
                "Left Join Participants on Participants.PeriodId = Periods.Id " +
                "Left Join ParticipantTeachers on ParticipantTeachers.ParticipantId = Participants.Id " +
                "Where ParticipantTeachers.TeacherId =  " + CurrentUser.Id+
                "Group by Periods.Id,Periods.Name,Periods.StartAt,Periods.FinishAt,Periods.CreateUserId,Periods.CreateAt";

            List<Period> result = _context.Periods.FromSqlRaw(Sql).ToList();


            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            User CurrentUser = _context.Users.Where(x => x.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();
            ViewData["CurrentUser"] = CurrentUser;

            if (id == null)
            {
                return NotFound();
            }

            Period period = await _context
                .Periods
                .Include(p => p.CreateUser)
                .Include(p => p.Participants)
                .Include("Participants.Student")
                .Include("Participants.ParticipantTeachers")
                .Include("Participants.ParticipantTeachers.Teacher")
                .FirstOrDefaultAsync(m => m.Id == id);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        public async Task<IActionResult> Participants(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User CurrentUser = _context.Users.Where(x => x.UserName == this.HttpContext.User.Identity.Name).FirstOrDefault();
            ViewData["CurrentUser"] = CurrentUser;

            Period period = await _context
                .Periods
                .Include(p => p.CreateUser)
                .Include(p => p.Participants)
                .Include("Participants.Student")
                .Include("Participants.ParticipantTeachers")
                .Include("Participants.ParticipantTeachers.Teacher")
                .FirstOrDefaultAsync(m => m.Id == id);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        public IActionResult Accept(int id)
        {
            var model = this._context.ParticipantTeachers.Find(id);

            model.Status = 1;
            this._context.Update(model);
            this._context.SaveChanges();

            return RedirectToAction("Index" );
        }

        public IActionResult Reject(int id)
        {
            var model = this._context.ParticipantTeachers.Find(id);

            model.Status = 2;
            this._context.Update(model);
            this._context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
