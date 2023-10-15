//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using FIT5032_IbrahimFinalProject.Data;

//namespace FIT5032_IbrahimFinalProject.Models
//{
//    public class DocumentsController : Controller
//    {
//        private readonly ClinicContext _context;

//        public DocumentsController(ClinicContext context)
//        {
//            _context = context;
//        }

//        // GET: Documents
//        public async Task<IActionResult> Index()
//        {
//              return _context.Documents != null ? 
//                          View(await _context.Documents.ToListAsync()) :
//                          Problem("Entity set 'ClinicContext.Documents'  is null.");
//        }

//        // GET: Documents/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Documents == null)
//            {
//                return NotFound();
//            }

//            var documents = await _context.Documents
//                .FirstOrDefaultAsync(m => m.ID == id);
//            if (documents == null)
//            {
//                return NotFound();
//            }

//            return View(documents);
//        }

//        // GET: Documents/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Documents/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Name")] Image image, HttpPostedFileBase postedFile)
//        {
//            ModelState.Clear();
//            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
//            image.Path = myUniqueFileName;
//            TryValidateModel(image);
//            if (ModelState.IsValid)
//            {
//                string serverPath = Server.MapPath("~/Uploads/");
//                string fileExtension = Path.GetExtension(postedFile.FileName);
//                string filePath = image.Path + fileExtension;
//                image.Path = filePath;
//                postedFile.SaveAs(serverPath + image.Path);
//                db.Images.Add(image);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//        }

//        // GET: Documents/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Documents == null)
//            {
//                return NotFound();
//            }

//            var documents = await _context.Documents.FindAsync(id);
//            if (documents == null)
//            {
//                return NotFound();
//            }
//            return View(documents);
//        }

//        // POST: Documents/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("ID,Path,Name")] Documents documents)
//        {
//            if (id != documents.ID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(documents);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!DocumentsExists(documents.ID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(documents);
//        }

//        // GET: Documents/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Documents == null)
//            {
//                return NotFound();
//            }

//            var documents = await _context.Documents
//                .FirstOrDefaultAsync(m => m.ID == id);
//            if (documents == null)
//            {
//                return NotFound();
//            }

//            return View(documents);
//        }

//        // POST: Documents/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Documents == null)
//            {
//                return Problem("Entity set 'ClinicContext.Documents'  is null.");
//            }
//            var documents = await _context.Documents.FindAsync(id);
//            if (documents != null)
//            {
//                _context.Documents.Remove(documents);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool DocumentsExists(int id)
//        {
//          return (_context.Documents?.Any(e => e.ID == id)).GetValueOrDefault();
//        }
//    }
//}
