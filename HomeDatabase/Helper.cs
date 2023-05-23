using System.Net.Mail;
using System.Net;

namespace HomeDatabase
{
    public class Helper
    {
        public void SMTP()
        {
            var client = new SmtpClient
            {
                Host = "smtp-relay.sendinblue.com",
                Port = 587, // or the appropriate port for your SMTP provider
                EnableSsl = true, // Enable SSL/TLS encryption
                Credentials = new NetworkCredential("taskoudisdimitris@gmail.com", "NZ0cqP31rFRT9gJL")
            };
        }


    }
}
