using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    [Area("Student")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
