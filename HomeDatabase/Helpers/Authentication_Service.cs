using HomeDatabase.Database;
using HomeDatabase.Models;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;


namespace HomeDatabase.Helpers
{
    public class Authentication_Service
    {

        public readonly EmailService emailService;
        public readonly GenerateToken tokenService;

        public Authentication_Service(GenerateToken tokenService, EmailService emailService)
        {
            this.tokenService = tokenService;
            this.emailService = emailService;
        }

        public string RegisterAccount(UsersViewModel model)
        {

            // Hash the password before saving to the database
            string passwordHash = Utils.HashPassword(model.Password);
            // Create a new user
            var newUser = new UsersViewModel
            {
                Username = model.Username,
                Password = passwordHash,
                PendingRegistration = true,
                Token = GenerateEmailVerificationToken(),
                Email = model.Email
            };

            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Email = '{model.Email}'");
            if (!(table.Rows.Count > 0))
            {
                SqlConnect.Instance.ExecuteNQ($"Insert Into Users Values ('{newUser.Username}', '{passwordHash}'" +
                $", '{newUser.Email}', '{newUser.Token}','{newUser.isAdmin}','{newUser.PendingRegistration}', '{newUser.resetPassword}')");
                // Send email verification link
                SendEmailVerificationEmail(newUser);
                // Generate a token for the registered user
                return tokenService.Generate_Token(newUser);
            }
            else
            {
                return null;
            }
        }

        public void SendEmailVerificationEmail(UsersViewModel user)
        {

            //Get my url

            // Construct an email with a verification link
            string verificationLink = $"https://localhost:7075/VerifyEmail?token={user.Token}";
            string subject = "Email Verification";
            string body = $"Click the following link to verify your email: {verificationLink}";

            // Send the email using your email service
            emailService.SendEmail(user.Email, subject, body);
        }

        public bool VerifyEmail(string emailVerificationToken)
        {
            // Implement verification logic without UserRepository
            // You might need to store user details in-memory or use a caching mechanism
            var user = GetUserByEmailVerificationToken(emailVerificationToken);

            if (user != null)
            {
                user.PendingRegistration = false;
                user.Token = null;

                SqlConnect.Instance.ExecuteNQ("Update Users set Token = null, PendingRegistration = false" +
                    $"where Token = {emailVerificationToken}");

                // Update the user details (in-memory or using a caching mechanism)
                // userRepo.UpdateUser(user);

                return true;
            }

            return false;
        }


        public string GenerateEmailVerificationToken()
        {
            // Generate a unique token for email verification
            // You can use a library like System.Security.Cryptography to create a secure random token
            byte[] tokenBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenBytes);
            }

            string emailVerificationToken = Convert.ToBase64String(tokenBytes)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty)
                .Replace("=", string.Empty);

            return emailVerificationToken;
        }

        private UsersViewModel user = new UsersViewModel();
        public UsersViewModel GetUserByEmailVerificationToken(string emailVerificationToken)
        {
            // This could be in-memory storage or using a caching mechanism
            // For simplicity, this example assumes a static list of users
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Token = '{emailVerificationToken}'");
            if (table.Rows.Count > 0)
            {
                foreach(DataRow row in table.Rows)
                {
                    user.Username = row["Username"].ToString();
                    user.Password = row["Password"].ToString();
                    user.Email = row["Email"].ToString();
                    user.Token = row["Token"].ToString();
                    user.isAdmin = Convert.ToBoolean(row["isAdmin"]);
                    user.PendingRegistration = Convert.ToBoolean(row["PendingRegistration"]);
                    user.resetPassword = Convert.ToBoolean(row["resetPassword"]);
                }
                return user;
            }
            else
            {
                return null;
            }
        }

        public UsersViewModel ValidateUser(UsersViewModel user)
        {
            UsersViewModel user_ = new UsersViewModel();
            DataTable table = SqlConnect.Instance.SelectDataTable($"Select * From Users where Username = '{user.Username}'");
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    if(Utils.VerifyPassword(user.Password, row["Password"].ToString()))
                    {
                        user_.Username = row["Username"].ToString();
                        user_.Email = row["Email"].ToString();
                        user_.Token = row["Token"].ToString();
                        user_.isAdmin = Convert.ToBoolean(row["isAdmin"]);
                        user_.PendingRegistration = Convert.ToBoolean(row["PendingRegistration"]);
                        user_.resetPassword = Convert.ToBoolean(row["resetPassword"]);
                    }
                }
                return user_;
            }
            else
            {
                return null;
            }


        }



    }
}
