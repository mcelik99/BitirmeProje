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
            var viewModel = new EnrollViewModel
            {
                Users = _context.Users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName()
                }).ToList(),
                Periods = _context.Periods.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Enroll(EnrollViewModel enrollViewModel)
        {
            /* if (ModelState.IsValid)
             {
                 foreach (var selectedUserId in enrollViewModel.SelectedUserIds)
                 {
                     var periodStudent = new PeriodStudent
                     {
                         Subject = enrollViewModel.Subject,
                         UserId = selectedUserId,
                         PeriodId = enrollViewModel.SelectedPeriodId
                     };

                     _context.PeriodStudents.Add(periodStudent);
                 }

                 _context.SaveChanges();

                 return RedirectToAction("Index", "Home");
             }

             enrollViewModel.Users = _context.Users
              .Select(u => new SelectListItem
                  {
                     Value = u.Id.ToString(),
                      Text = u.FullName()
                      })
                      .ToList();

             enrollViewModel.Periods = _context.Periods
               .Select(p => new SelectListItem
               {
                   Value = p.Id.ToString(),
                    Text = p.Name
              })
              .ToList();
            */
            return View();
        }

        
    }
}
