using BitirmeProjesi.Data;
using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BitirmeProjesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PeriodController : Controller
    {
        private readonly BitirmeDBContext _context;

        public PeriodController(BitirmeDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Period> periods = await _context.Periods.Include(x => x.CreateUser).ToListAsync();
            return View(periods);
        }

        // GET: Periods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Periods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Periods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartAt,FinishAt")] Period period)
        {
            if (ModelState.IsValid)
            {
                period.CreateAt = DateTime.Now;
                period.CreateUserId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                _context.Add(period);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(period);
        }

        // GET: Periods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Period period = await _context.Periods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }
            return View(period);
        }

        // POST: Periods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartAt,FinishAt,CreateAt,CreateUserId")] Period period)
        {
            if (id != period.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(period);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodExists(period.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(period);
        }

        // GET: Periods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Period period = await _context.Periods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Period period = await _context.Periods.FindAsync(id);
            _context.Periods.Remove(period);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodExists(int id)
        {
            return _context.Periods.Any(e => e.Id == id);
        }

        public IActionResult Accept(int id)
        {
            var model = this._context.Participants.Find(id);

            model.AdvisorStatus = 1;
            this._context.Update(model);
            this._context.SaveChanges();

            return RedirectToAction("Details", new { id = model.PeriodId });
        }
        
        public IActionResult Reject(int id)
        {
            var model = this._context.Participants.Find(id);

            model.AdvisorStatus = 2;
            this._context.Update(model);
            this._context.SaveChanges();

            return RedirectToAction("Details", new { id = model.PeriodId });
        }
    }
}




