using BitirmeProjesi.Data;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PeriodController : Controller
    {
        private readonly BitirmeDBContext BitirmeDBContext;
        public PeriodController(BitirmeDBContext BitirmeDBContext)
        {
            this.BitirmeDBContext = BitirmeDBContext;
        }

        public IActionResult Index()
        {
            int UserID = 1;

            return View();
        }
    }
}
