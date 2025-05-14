using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using labweek5_form_project.Models;
using System.Text.Json;

namespace labweek5_form_project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User Input { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");

            if (!System.IO.File.Exists(filePath))
            {
                ErrorMessage = "User data not found.";
                return Page();
            }

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            var user = users.FirstOrDefault(u =>
                u.Username == Input.Username &&
                u.Password == Input.Password &&
                u.IsActive);

            if (user == null)
            {
                ErrorMessage = "Username or password is incorrect.";
                return Page();
            }

            var token = Guid.NewGuid().ToString();

            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

            var options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("username", user.Username, options);
            Response.Cookies.Append("token", token, options);
            Response.Cookies.Append("session_id", HttpContext.Session.Id, options);

            return RedirectToPage("Index");
        }
    }
}
