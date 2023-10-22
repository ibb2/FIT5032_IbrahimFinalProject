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
        private string webRootPath;
        private string uploadsPath;
        private string fileExtension;
        private string filePath;
        private string fullPath;

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

            // Save this file in order to upload. Idea came from someone having upload issues on forums

            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            email.Path = myUniqueFileName;

            Boolean doesAttachmentExist = file.FileName != null;

            if (doesAttachmentExist)
            {
                webRootPath = _webHostEnvironment.WebRootPath;
                uploadsPath = Path.Combine(webRootPath, "uploads");

                fileExtension = Path.GetExtension(file.FileName);
                filePath = email.Path + fileExtension;

                email.Path = filePath;

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                fullPath = Path.Combine(uploadsPath, filePath);


                // Code adapted from https://stackoverflow.com/questions/73720188/how-to-save-files-to-another-folder-in-asp-net-core
                using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    file.CopyToAsync(fs);
                }

                //postedFile.SaveAs(serverPath + image.Path);
                //db.Images.Add(image);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }


            // Sendgrid emailing, default from there github repo https://github.com/sendgrid/sendgrid-csharp
            // This will actually send out the email
            // As sendgrid does not allow unverified senders to send emails the suggestion from 
            // https://stackoverflow.com/questions/68399298/sendgrid-mail-wont-allow-sending-from-to-be-dynamically 
            // will be used to work around this issue
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ibbs824@gmail.com", "uMRI Verified Sender"),
                Subject = email.Subject,
                PlainTextContent = email.Content,
            };
            if (doesAttachmentExist && fullPath != null) {
                // How to convert to bytes https://stackoverflow.com/questions/25919387/converting-file-into-base64string-and-back-again
                // File error fixed with https://stackoverflow.com/questions/60927331/controllerbase-filebyte-string-is-a-method-which-is-not-valid-in-the-giv
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                var fileBase64 = Convert.ToBase64String(bytes);
                msg.AddAttachment(filename: fullPath, base64Content: fileBase64, type: file.ContentType);
            }
            // This value is hardcoded for the mail address of the clinic.
            // In this case it is just my personal email
            msg.AddReplyTo( new EmailAddress(email.From, User.Identity.Name));
            msg.AddTo(new EmailAddress("ibyster824@gmail.com", "uMRI Team"));
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);


            ModelState.Clear();
            email.To = "ibyster824@gmail.com";

            if (ModelState.IsValid && response.IsSuccessStatusCode)
            {
                _context.Add(email);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
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
