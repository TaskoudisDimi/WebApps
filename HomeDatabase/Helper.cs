using System.Net.Mail;
using System.Net;


namespace HomeDatabase
{
    public class Helper
    {
        private static readonly IWebHostEnvironment _env;
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


        public static void Log(string message,string title)
        {
            try
            {
                string appRootPath = AppContext.BaseDirectory;
                string path = appRootPath + $"{title}.txt";
                File.WriteAllText(path, message);
            }
            catch
            {

            }
        }




    }
}
