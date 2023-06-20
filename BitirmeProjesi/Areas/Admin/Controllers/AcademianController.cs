using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                "Where ParticipantTeachers.TeacherId =  " + CurrentUser.Id +
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

            ViewBag.Message = TempData["Message"];

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
            ParticipantTeacher model = this._context.ParticipantTeachers.Include(x => x.Participant).Include(y => y.Teacher).Where(z=>z.Id == id).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            int onaySayisi = _context.ParticipantTeachers.Count(pt => pt.TeacherId == model.TeacherId && pt.Status == 1 && pt.Participant.PeriodId == model.Participant.PeriodId);
            if (model.Teacher.Quota > onaySayisi)
            {

                model.Status = 1;
                this._context.Update(model);

            }
            else
            {
                ViewBag.Message = "Kota dolu.";
            }
            this._context.SaveChanges();

            TempData["Message"] = ViewBag.Message;
            return RedirectToAction("Participants", new { id = model.Participant.PeriodId, message = ViewBag.Message });

        }


        public IActionResult Reject(int id)
        {
            var model = this._context.ParticipantTeachers.Find(id);

            model.Status = 2;
            this._context.Update(model);
            this._context.SaveChanges();

            if (model != null)
            {
                var participantId = model.ParticipantId;
                var participant = _context.Participants.FirstOrDefault(p => p.Id == participantId);
                if (participant != null)
                {
                    var periodId = participant.PeriodId;
                    var period = _context.Periods.FirstOrDefault(p => p.Id == periodId);
                    if (period != null)
                    {
                        return RedirectToAction("Participants", new { id = period.Id });
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
