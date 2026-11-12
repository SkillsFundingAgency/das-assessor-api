using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public class AssessorHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultsDescription = "Assessor API Health Check";
        private readonly IUserEntityContext _userEntityContext;

        public AssessorHealthCheck(IUserEntityContext userEntityContext)
        {
            _userEntityContext = userEntityContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbConnectionHealthy = true;
            try
            {
                await _userEntityContext.GetFirstOrDefault();
            }
            catch
            {
                dbConnectionHealthy = false;
            }

            return dbConnectionHealthy ? HealthCheckResult.Healthy(HealthCheckResultsDescription) : HealthCheckResult.Unhealthy(HealthCheckResultsDescription);
        }
    }
}