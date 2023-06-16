using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StudentListController : Controller
    {
        private readonly BitirmeDBContext _context;

        public StudentListController(BitirmeDBContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
           
            var students = await _context.Students.ToListAsync();

            return View(students);
        }
    }
}