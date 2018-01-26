using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private const string _mailTo = "admin@mycompany.com";
        private const string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
