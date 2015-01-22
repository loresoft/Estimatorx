using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public class EmailService : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(message.Destination);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            
            var smtpClient = new SmtpClient();            
            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}