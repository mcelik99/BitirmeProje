using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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



        // Yeni bir Period oluşturur
        [HttpPost]
        public ActionResult Create(Period period)
        {
            if (ModelState.IsValid)
            {
                
                BitirmeDBContext.Periods.Add(period);

               
                BitirmeDBContext.SaveChanges();
                
            }

            return View(period);
        }


        public ActionResult Detail(int id)
        {
           var period = BitirmeDBContext.Periods.FirstOrDefault(p => p.Id == id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }


        public IActionResult Index()
        {
             int UserID = 1;

            var periods = BitirmeDBContext.Periods.ToList();
            return View(periods);
        }


    }
}
