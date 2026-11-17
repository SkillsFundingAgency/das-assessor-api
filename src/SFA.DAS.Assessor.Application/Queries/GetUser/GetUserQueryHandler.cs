using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly IUserEntityContext _userContext;

        public GetUserQueryHandler(IUserEntityContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<GetUserQueryResult> Handle(
            GetUserQuery request,
            CancellationToken cancellationToken)
        {
            var user = request.DfeSignInUserId is null
                ? null
                : await _userContext.Get(request.DfeSignInUserId);

            return new GetUserQueryResult
            {
                User = (Domain.Models.User?)user
            };
        }
    }
}
