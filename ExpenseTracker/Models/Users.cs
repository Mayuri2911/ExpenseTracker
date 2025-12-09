namespace ExpenseTracker.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Plain — will hash before save
    }
}
