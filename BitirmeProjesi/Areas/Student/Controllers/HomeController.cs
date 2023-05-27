using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Student")]
    public class HomeController : BaseController
    {
        private readonly BitirmeDBContext _context;

        public HomeController(BitirmeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

       
    }
}
