using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FIT5032_IbrahimFinalProject.Data;
using FIT5032_IbrahimFinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FIT5032_IbrahimFinalProject.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ClinicContext _context;

        public RatingsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.Ratings.Include(r => r.Customer);
            return View(await clinicContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Ratings/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "DOB");
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CustomerID,StaffID,RatingScore,Message,Reply,Location")] Rating rating)
        {
            ModelState.Clear();

            var customer = _context.Customers.FirstOrDefault(c => c.UserId == User.Identity.GetUserId());
            rating.CustomerID = customer.ID;

            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "DOB", rating.CustomerID);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings.FindAsync(id);
            var customerId = _context.Customers.FirstOrDefault(c => c.UserId == User.Identity.GetUserId()).ID;
            if (rating.CustomerID != customerId)
            {
                ViewData["NotYou"] = "You do not have the right to edit";
                return RedirectToAction(nameof(Index));
            }
            if (rating == null)
            {
                return NotFound();
            } 
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "DOB", rating.CustomerID);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CustomerID,StaffID,RatingScore,Message,Reply,Location")] Rating rating)
        {
            if (id != rating.ID)
            {
                return NotFound();
            }

            ModelState.Clear();

            var customer = _context.Customers.FirstOrDefault(c => c.UserId == User.Identity.GetUserId());
            rating.CustomerID = customer.ID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.ID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "DOB", rating.CustomerID);
            return View(rating);
        }

        [Authorize]
        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);

            var rate = await _context.Ratings.FindAsync(id);
            var customerId = _context.Customers.FirstOrDefault(c => c.UserId == User.Identity.GetUserId()).ID;
            if (rate.CustomerID != customerId)
            {
                ViewData["NotYou"] = "You do not have the right to delete";
                return RedirectToAction(nameof(Index));
            }
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ratings == null)
            {
                return Problem("Entity set 'ClinicContext.Ratings'  is null.");
            }
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return (_context.Ratings?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
