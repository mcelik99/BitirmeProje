using BitirmeProjesi.Areas.Student.Models.Dto;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using BitirmeProjesi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Student")]
    public class ParticipantController : BaseController
    {

        private readonly BitirmeDBContext _context;

        public ParticipantController(BitirmeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int StudentId = this.getStudentID();

            var participants = _context.Participants
                .Include(y => y.Period)
                .Include(z => z.ParticipantTeachers)
                .Include(z => z.ParticipantTeachers)
                .Include("ParticipantTeachers.Teacher")
                .Where(x => x.StudentId == StudentId)
                .ToList();

            return View(participants);
        }

    }
}
