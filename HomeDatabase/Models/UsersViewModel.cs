using Microsoft.AspNetCore.Authentication;

namespace HomeDatabase.Models
{
    public class UsersViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public bool isAdmin { get; set; }

        public bool PendingRegistration { get; set; }

        public bool resetPassword { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


    }
}
