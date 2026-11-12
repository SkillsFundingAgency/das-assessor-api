using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.Assessor.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public class NoneRequirement : IAuthorizationRequirement
    {
    }
}
