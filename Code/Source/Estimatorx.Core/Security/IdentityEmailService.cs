using System.Net.Mail;
using Microsoft.AspNet.Identity;
using NLog.Fluent;

namespace Estimatorx.Core.Security
{
    public class IdentityEmailService : IIdentityMessageService
    {

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(message.Destination);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = true;

            _logger.Info()
                .Message("Send Email to '{0}' with Subject '{1}'", message.Destination, message.Subject)
                .Write();

            var smtpClient = new SmtpClient();            
            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}