using BitirmeProjesi.Areas.Student.Models.Dto;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Student")]
    public class PeriodController : BaseController
    {

        private readonly BitirmeDBContext _context;

        public PeriodController(BitirmeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var periods = _context.Periods.Where(x => x.FinishAt > DateTime.Now).ToList();

            return View(periods);
        }

        [HttpGet]
        public IActionResult Participant(int id)
        {
            int StudentId = (int)this.HttpContext.Session.GetInt32("STUDENT_ID");
            
            if (_context.Participants.Any(x => x.StudentId == StudentId && x.PeriodId == id))
            {
                return RedirectToAction("Index", "Participant", new { area = "Student" });
            }
        
            var teachers = _context.Users.Where(x => x.IsAdvisor == true).ToList();

            ViewData["teachers"] = teachers;

            return View(new ParticipantDto());
        }

        [HttpPost]
        public IActionResult Participant(int id, ParticipantDto Model)
        {
            var teachers = _context.Users.Where(x => x.IsAdvisor == true).ToList();

            ViewData["teachers"] = teachers;

            if (ModelState.IsValid)
            {
                Participant Participant = new Participant();
                Participant.Subject = Model.Subject;
                Participant.PeriodId = id;
                Participant.StudentId = (int)this.HttpContext.Session.GetInt32("STUDENT_ID");
                Participant.AdvisorStatus = 0;
                Participant.CreateAt = DateTime.Now;

                _context.Participants.Add(Participant);
                _context.SaveChanges();


                for (byte i = 0; i < Model.Teachers.Count; i++)
                {
                    ParticipantTeacher ParticipantTeacher = new ParticipantTeacher();
                    ParticipantTeacher.ParticipantId = Participant.Id;
                    ParticipantTeacher.TeacherId = Model.Teachers[i];
                    ParticipantTeacher.Status = 0;
                    ParticipantTeacher.Direction = i;

                    _context.ParticipantTeachers.Add(ParticipantTeacher);
                }

                _context.SaveChanges();

                return RedirectToAction("Index", "Participant", new { area = "Student" });

            }


            return View(Model);
        }


    }
}
