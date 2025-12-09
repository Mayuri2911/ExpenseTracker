using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

            var chartData = selectedtransactions
      .Where(i => i.Categories.Type == "Expense")
      .GroupBy(j => j.Categories.CategoryId)
      .Select(k => new
      {
          CategoryTitleWithIcon = k.First().CategoryTitleWithIcon,
          Amount = k.Sum(j => j.Amount),
          FormattedAmount = k.Sum(j => j.Amount).ToString("C0")
      })
      .ToList();

            ViewBag.CategoryTitleWithIcon = chartData.Select(x => x.CategoryTitleWithIcon).ToList();
            ViewBag.Values = chartData.Select(x => x.Amount).ToList();
            ViewBag.FormattedValues = chartData.Select(x => x.FormattedAmount).ToList();


            // --- Income vs Expense for last 7 days ---
            var last7days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i))
                .ToList();

            var incomeData = last7days.Select(day =>
                selectedtransactions.Where(t => t.Date.Date == day.Date && t.Categories.Type == "Income")
                .Sum(t => t.Amount)
            ).ToList();

            var expenseData = last7days.Select(day =>
                selectedtransactions.Where(t => t.Date.Date == day.Date && t.Categories.Type == "Expense")
                .Sum(t => t.Amount)
            ).ToList();

            ViewBag.Days = last7days.Select(d => d.ToString("dd MMM")).ToList();
            ViewBag.Income7 = incomeData;
            ViewBag.Expense7 = expenseData;


            ViewBag.RecentTransaction = await _context.Transactions
           .Include(i => i.Categories)
           .OrderByDescending(j => j.Date)
           .Take(5)
           .ToListAsync();

            return View();
        }


    }
}
