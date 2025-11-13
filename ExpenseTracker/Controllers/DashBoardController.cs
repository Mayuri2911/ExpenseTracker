using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashBoardController(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index()
        {

            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;


            List<Transaction> selectedtransactions = await _context.Transactions.Include(x => x.Categories)
           .Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();


            int TotalIncome = selectedtransactions
          .Where(i => i.Categories != null && i.Categories.Type == "Income")
          .Sum(j => j.Amount);

            int TotalExpense = selectedtransactions
                .Where(i => i.Categories != null && i.Categories.Type == "Expense")
                .Sum(j => j.Amount);




            //Total Balance 
            int TotalBalance = TotalIncome - TotalExpense;
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            cultureInfo.NumberFormat.CurrencyNegativePattern = 1;
            // ViewBag.TotalIncome = TotalIncome.ToString("C0");
            //ViewBag.TotalExpense = TotalExpense.ToString("C0");
            ViewBag.TotalIncome = string.Format(cultureInfo, "{0:C0}", TotalIncome);
            ViewBag.TotalExpense = string.Format(cultureInfo, "{0:C0}", TotalExpense);
            ViewBag.TotalBalance = string.Format(cultureInfo, "{0:C0}", TotalBalance);
             //   TotalBalance.ToString("C0");




            return View();
        }
    }
}
