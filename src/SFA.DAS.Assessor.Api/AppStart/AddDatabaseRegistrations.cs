using System;
using System.Diagnostics.CodeAnalysis;
using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Assessor.Data;
using SFA.DAS.Assessor.Domain.Configuration;

namespace SFA.DAS.Assessor.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddDatabaseRegistrations
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            if (configuration.IsLocalAcceptanceOrDev())
            {
                services.AddDbContext<AssessorDataContext>(options => options.UseSqlServer(appSettings?.DbConnectionString).EnableSensitiveDataLogging(), ServiceLifetime.Scoped);
            }
            else if (configuration.IsIntegrationTests())
            {
                services.AddDbContext<AssessorDataContext>(options => options.UseSqlServer("Server=localhost;Database=SFA.DAS.Assessor.IntegrationTests.Database;Trusted_Connection=True;MultipleActiveResultSets=true").EnableSensitiveDataLogging(), ServiceLifetime.Scoped);
            }
            else
            {
                services.AddSingleton(new ChainedTokenCredential(
                                 new ManagedIdentityCredential(new ManagedIdentityCredentialOptions()),
                                 new AzureCliCredential(),
                                 new VisualStudioCodeCredential(),
                                 new VisualStudioCredential())
                         );
                services.AddDbContext<AssessorDataContext>(ServiceLifetime.Scoped);
            }

            services.AddScoped(provider => new Lazy<AssessorDataContext>(() => provider.GetRequiredService<AssessorDataContext>()));
        }
    }
}
