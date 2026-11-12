using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Assessor.Api.TaskQueue;
using SFA.DAS.Assessor.Application.Behaviours;
using SFA.DAS.Assessor.Application.Queries.GetUser;
using SFA.DAS.Assessor.Data;
using SFA.DAS.Assessor.Domain.Interfaces;
using SFA.DAS.Encoding;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Assessor.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));

            services.AddValidatorsFromAssemblyContaining<GetUserQuery>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<IUserEntityContext>(s => s.GetRequiredService<AssessorDataContext>());
            services.AddSingleton<IEncodingService, EncodingService>();
        }
    }
}