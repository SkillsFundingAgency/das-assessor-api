using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Assessor.Api.TaskQueue;
using SFA.DAS.Assessor.Application.Behaviours;
using SFA.DAS.Assessor.Application.Queries.GetUser;
using SFA.DAS.Assessor.Data;
using SFA.DAS.Assessor.Domain.Interfaces;

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

            services.AddScoped<IUserEntityContext, UserEntityContext>();
        }
    }
}