using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }

        [Column (TypeName ="nvarchar(50)")]
        [Required(ErrorMessage ="This Feild is Required")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + "" + this.Title;
            }
        
        }
        public ICollection<Transaction>? Transactions { get; set; }

    }
}
