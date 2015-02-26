using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public class IdentityEmailService : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(message.Destination);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient();            
            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}