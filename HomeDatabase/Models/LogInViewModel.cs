using Microsoft.AspNetCore.Authentication;

namespace HomeDatabase.Models
{
    public class LogInViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}
