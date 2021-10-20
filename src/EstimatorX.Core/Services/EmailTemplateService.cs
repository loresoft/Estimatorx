using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EstimatorX.Core.Extensions;
using EstimatorX.Core.Models;
using EstimatorX.Core.Options;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EstimatorX.Core.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IOptions<SendGridConfiguration> _sendGridOptions;
        private readonly ILogger<EmailTemplateService> _logger;

        public EmailTemplateService(IOptions<SendGridConfiguration> sendGridOptions, ISendGridClient sendGridClient, ILogger<EmailTemplateService> logger)
        {
            _sendGridOptions = sendGridOptions;
            _sendGridClient = sendGridClient;
            _logger = logger;
        }


        public async Task<bool> SendResetPasswordEmail(UserResetPasswordEmail resetPassword)
        {
            if (resetPassword == null)
                throw new ArgumentNullException(nameof(resetPassword));

            var emailTemplate = GetEmailTemplate(Templates.ResetPassword);

            return await SendTemplate(emailTemplate, resetPassword).ConfigureAwait(false);
        }

        public async Task<bool> SendPasswordlessLoginEmail(UserPasswordlessEmail loginEmail)
        {
            if (loginEmail == null)
                throw new ArgumentNullException(nameof(loginEmail));

            var emailTemplate = GetEmailTemplate(Templates.PasswordlessLogin);

            return await SendTemplate(emailTemplate, loginEmail).ConfigureAwait(false);
        }


        public async Task<bool> SendTemplate<TModel>(EmailTemplate emailTemplate, TModel emailModel)
            where TModel : EmailModelBase
        {
            if (emailTemplate == null)
                throw new ArgumentNullException(nameof(emailTemplate));

            if (emailModel == null)
                throw new ArgumentNullException(nameof(emailModel));

            try
            {
                var subject = ApplyTemplate(emailTemplate.Subject, emailModel);
                var htmlBody = ApplyTemplate(emailTemplate.HtmlBody, emailModel);
                var textBody = ApplyTemplate(emailTemplate.TextBody, emailModel);

                // first try reply to name, next try model from address, default to option address
                var fromName = emailTemplate.ReplyToName.HasValue()
                    ? emailTemplate.ReplyToName
                    : emailTemplate.FromName.HasValue()
                        ? emailTemplate.FromName
                        : _sendGridOptions.Value.FromName;

                var fromEmail = emailTemplate.FromAddress.HasValue()
                    ? emailTemplate.FromAddress
                    : _sendGridOptions.Value.FromEmail;

                var fromAddress = new EmailAddress(fromEmail, fromName);

                var message = new SendGridMessage();
                message.From = fromAddress;

                if (emailTemplate.ReplyToAddress.HasValue())
                    message.ReplyTo = new EmailAddress(emailTemplate.ReplyToAddress, emailTemplate.ReplyToName);

                message.AddTo(new EmailAddress(emailModel.RecipientAddress, emailModel.RecipientName));

                message.Subject = subject;
                message.PlainTextContent = textBody;
                message.HtmlContent = htmlBody;

                _logger.LogInformation("Sending Email To: {email}, Subject: {subject} ...", emailModel.RecipientAddress, subject);

                var response = await _sendGridClient.SendEmailAsync(message).ConfigureAwait(false);

                _logger.LogInformation("Sent Email To: {email}, Subject: {subject}: Status Code: {statusCode}", emailModel.RecipientAddress, subject, response.StatusCode);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Sending Email To: {email}; {message}", emailModel.RecipientAddress, ex.Message);
                throw;
            }
        }


        private string ApplyTemplate<TModel>(string handlebarTemplate, TModel model)
        {
            if (handlebarTemplate.IsNullOrWhiteSpace())
                return string.Empty;

            var compiledTemplate = Handlebars.Compile(handlebarTemplate);
            var result = compiledTemplate(model);

            return result;
        }

        private EmailTemplate GetEmailTemplate(string templateKey)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"EstimatorX.Core.Templates.{templateKey}.yml";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                return null;

            using var reader = new StreamReader(stream);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<EmailTemplate>(reader);
        }


        public static class Templates
        {
            public const string ResetPassword = "reset-password";
            public const string PasswordlessLogin = "passwordless-login";
        }

    }

}
