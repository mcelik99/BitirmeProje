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
            var model = this._context.ParticipantTeachers.Find(id);
            var user = _context.Users.FirstOrDefault(u => u.Id == model.TeacherId);
            if (user != null && user.Quota > 0)
            {
                // Kullanıcının kotasını 1 azalt
                user.Quota--;
                _context.Update(user);
                _context.SaveChanges();


                model.Status = 1;
                this._context.Update(model);

                ViewBag.Message = "Kota azaltıldı.  Kalan Kota: " + user.Quota.ToString();

            }
            else
            {
                ViewBag.Message = "Kota dolu.";
            }

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
                        TempData["Message"] = ViewBag.Message;
                        return RedirectToAction("Participants", new { id = period.Id, message = ViewBag.Message });

                    }
                }
            }
            return RedirectToAction("Index");
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
