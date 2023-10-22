using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FIT5032_IbrahimFinalProject.Data;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace FIT5032_IbrahimFinalProject.Models
{
    public class EmailsController : Controller
    {
        private readonly ClinicContext _context;

        // Server.MapPath seems deprecated 
        // Answer found from https://stackoverflow.com/questions/49398965/what-is-the-equivalent-of-server-mappath-in-asp-net-core
        // Related code referenes the above plus microsoft documentation
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailsController(ClinicContext context, IWebHostEnvironment webHostingEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostingEnvironment;
        }

        // GET: Emails
        public async Task<IActionResult> Index()
        {
              return _context.Email != null ? 
                          View(await _context.Email.ToListAsync()) :
                          Problem("Entity set 'ClinicContext.Email'  is null.");
        }

        // GET: Emails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Email == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .FirstOrDefaultAsync(m => m.ID == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // GET: Emails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,From,To,Subject,Content,Path,FileName")] Email email, IFormFile file)
        {

            //ModelState.Clear();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            email.Path = myUniqueFileName;

            if (file.FileName != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadsPath = Path.Combine(webRootPath, "uploads");

                string fileExtension = Path.GetExtension(file.FileName);
                string filePath = email.Path + fileExtension;

                email.Path = filePath;

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }


                // Code adapted from https://stackoverflow.com/questions/73720188/how-to-save-files-to-another-folder-in-asp-net-core
                using (var fs = new FileStream(Path.Combine(uploadsPath, filePath), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    file.CopyToAsync(fs);
                }

                //postedFile.SaveAs(serverPath + image.Path);
                //db.Images.Add(image);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            // Sendgrid emailing, default from there github repo https://github.com/sendgrid/sendgrid-csharp
            //var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            //var client = new SendGridClient(apiKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("test@example.com", "DX Team"),
            //    Subject = "Sending with Twilio SendGrid is Fun",
            //    PlainTextContent = "and easy to do anywhere, even with C#",
            //    HtmlContent = "<strong>and easy to do anywhere, even with C#</strong>"
            //};
            //msg.AddTo(new EmailAddress("test@example.com", "Test User"));
            //var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            if (ModelState.IsValid)
            {
                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(email);
        }

        // GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Email == null)
            {
                return NotFound();
            }

            var email = await _context.Email.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,From,To,Subject,Content,Path,FileName")] Email email)
        {
            if (id != email.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(email.ID))
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
            return View(email);
        }

        // GET: Emails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Email == null)
            {
                return NotFound();
            }

            var email = await _context.Email
                .FirstOrDefaultAsync(m => m.ID == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Email == null)
            {
                return Problem("Entity set 'ClinicContext.Email'  is null.");
            }
            var email = await _context.Email.FindAsync(id);
            if (email != null)
            {
                _context.Email.Remove(email);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(int id)
        {
          return (_context.Email?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
