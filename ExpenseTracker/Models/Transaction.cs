using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{

    [Table("Transactions", Schema = "dbo")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public Categories? Categories { get; set; }
        public int Amount { get; set; }
        [Column(TypeName = "nvarchar(70)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Categories == null ? " " : Categories.Icon + " " + Categories.Title;
            }

        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return (Categories == null || Categories.Type == "Expense" ? "- " : "+ ")
                       + Amount.ToString("C0");
            }
        }

    }
}

