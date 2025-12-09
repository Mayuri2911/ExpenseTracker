using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        // ---------------- REGISTER -----------------
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (!ModelState.IsValid) return View(user);

            using var con = new SqlConnection(_config.GetConnectionString("DevConnection"));
            con.Open();

            // Check if email already exists
            var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @e", con);
            checkCmd.Parameters.AddWithValue("@e", user.Email);
            int exists = (int)checkCmd.ExecuteScalar();
            if (exists > 0)
            {
                ViewBag.Error = "User already exists. You can login.";
                return View(user);
            }

            // Insert new user
            var cmd = new SqlCommand(
                "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@n, @e, @p)", con);
            cmd.Parameters.AddWithValue("@n", user.FullName);
            cmd.Parameters.AddWithValue("@e", user.Email);
            cmd.Parameters.AddWithValue("@p", user.Password);
            cmd.ExecuteNonQuery();

            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // ---------------- LOGIN -----------------
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Users user = null;

            using var con = new SqlConnection(_config.GetConnectionString("DevConnection"));
            con.Open();

            var cmd = new SqlCommand(
                "SELECT FullName, Email FROM Users WHERE Email=@e AND PasswordHash=@p", con);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@p", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new Users
                {
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString()
                };
            }

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // Generate JWT Token
            string token = GenerateJwtToken(user);

            // Store token in HttpOnly Secure Cookie
            Response.Cookies.Append("JWTToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // set false if testing on HTTP localhost
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            return RedirectToAction("Index", "Dashboard");
        }

        // ---------------- LOGOUT -----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");

            // Redirect to Login page
            return RedirectToAction("Login", "Account");
        }

        // ---------------- JWT GENERATOR -----------------
        private string GenerateJwtToken(Users user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
