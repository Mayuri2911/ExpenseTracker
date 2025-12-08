using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure;
using Syncfusion.EJ2.Linq;

namespace ExpenseTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {


            int totalItems = await _context.Transactions.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page > totalPages) page = totalPages;
            if (page < 1) page = 1;

  
            var transaction = await _context.Transactions
               .Include(t => t.Categories)   
              .OrderByDescending(t => t.Date)
                .ThenBy(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); ;

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
         //   var transaction = _context.Transactions;
            return View( transaction);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Transaction transaction = new Transaction();

            if (id != 0)
            {
                transaction = _context.Transactions
                                      .FirstOrDefault(t => t.TransactionId == id);
                if (transaction == null)
                    return NotFound();
            }
            PrepopulateCategories(transaction.CategoryId);

            return View(transaction);
        }



        // POST: Transactions/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                    _context.Add(transaction);   // Add new
                else
                    _context.Update(transaction); // Update existing

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PrepopulateCategories(transaction.CategoryId);
            return View(transaction);
        }



        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = await _context.Transactions
               // .Include(t => t.Categories)
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

   
        [NonAction]
        public void PrepopulateCategories(int? selectedCategoryId = null)
        {
            // Get all categories from database
            var categoriesCollection = _context.Categories.ToList();

            // Add default option at the top
            Categories defaultCategory = new Categories()
            {
                CategoryId = 0,
                Title = "Select Category",
                Icon = "" 
            };
            categoriesCollection.Insert(0, defaultCategory);
          
            ViewBag.Categories = new SelectList(
                categoriesCollection,  
                "CategoryId",           
                "TitleWithIcon",      
                selectedCategoryId      // selected value
            );
        }



        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
