using System;
using System.Collections.Generic;
using EstimatorX.Core.Options;
using KickStart.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace EstimatorX.Core.Services
{
    public class ServiceDependencyRegistration : IDependencyInjectionRegistration
    {
        public void Register(IServiceCollection services, IDictionary<string, object> data)
        {
            data.TryGetValue(ConfigurationDependencyRegistration.ConfigurationKey, out var configurationData);

            if (!(configurationData is IConfiguration configuration))
                throw new InvalidOperationException("Count not find configuration object");

            var sendGridConfiguration = configuration
                .GetSection(SendGridConfiguration.ConfigurationName)
                .Get<SendGridConfiguration>();

            services.AddSendGrid(options => options.ApiKey = sendGridConfiguration.ApiKey);

            services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
        }
    }
}
