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
    public class BookingsController : Controller
    {
        private readonly ClinicContext _context;

        public BookingsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Bookings

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Users"))
            {
                string currentUserId = User.Identity.GetUserId();
                var currentCustomer = _context.Customers.FirstOrDefault(c => c.UserId == currentUserId);

                var clinicContext = _context.Bookings.Where(b => b.Customer == currentCustomer).Include(b => b.Customer);
                return View(await clinicContext.ToListAsync());
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            {
                var clinicContext = _context.Bookings.Include(b => b.Customer);
                return View(await clinicContext.ToListAsync());
            } else
            {
                var clinicContext = _context.Bookings;
                return View(await clinicContext.ToListAsync());
            }

        }

        // GET: Bookings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID, CustomerID, BookingDate")] Booking booking)
        {

            var currentCustomer = _context.Customers.FirstOrDefault(u => u.UserId == User.Identity.GetUserId());
            ModelState.Clear();
            TryValidateModel(booking);

            if (ModelState.IsValid)
            {
                booking.Customer = currentCustomer;
                booking.CustomerID = currentCustomer.ID;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", booking.CustomerID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", booking.CustomerID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CustomerID,BookingDate")] Booking booking)
        {
            if (id != booking.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.ID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "ID", booking.CustomerID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ClinicContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
