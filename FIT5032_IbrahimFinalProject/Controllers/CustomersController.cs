﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FIT5032_IbrahimFinalProject.Data;
using FIT5032_IbrahimFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace FIT5032_IbrahimFinalProject.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ClinicContext _context;

        public CustomersController(ClinicContext context )
        {
            _context = context;
        }

        // GET: Customers
        [Authorize]
        public async Task<IActionResult> Index()

        {
            if (User.IsInRole("Users"))
            {
                string currentUserId = User.Identity.GetUserId();
                return _context.Customers != null ?
                            View(await _context.Customers.Where(
                                u => u.UserId == currentUserId).ToListAsync()) :
                            Problem("Entity set 'ClinicContext.Customers'  is null.");
            } else
            {
                return _context.Customers != null ?
                            View(await _context.Customers.ToListAsync()) :
                            Problem("Entity set 'ClinicContext.Customers'  is null.");
            }
            //return _context.Customers != null ?
            //            View(await _context.Customers.ToListAsync()) :
            //            Problem("Entity set 'ClinicContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            Boolean doesUserAlreadyHaveCustomer = _context.Customers.Where(c => c.UserId == User.Identity.GetUserId()).Count() > 0;

            if (doesUserAlreadyHaveCustomer)
            {
                TempData["UserExists"] = "A Customer with your user already exists.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,PhoneNo,DOB,FirstName,LastName")] Customer customer)
        {

            Boolean doesUserAlreadyHaveCustomer = _context.Customers.Where(c => c.UserId == User.Identity.GetUserId()).Count() > 0;

            if (!doesUserAlreadyHaveCustomer)
            {
                ModelState.Clear();
                customer.UserId = User.Identity.GetUserId();
                TryValidateModel(customer);
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            } else
            {
                ModelState.AddModelError("UserId", "A customer with your user already exists.");
                ViewData["UserExists"] = "A Customer with your user already exists.";
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Email,PhoneNo,DOB,FirstName,LastName")] Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "Staff, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ClinicContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
