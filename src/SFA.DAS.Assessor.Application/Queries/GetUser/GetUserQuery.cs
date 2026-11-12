using MediatR;

namespace SFA.DAS.Assessor.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserQueryResult>
    {
        public string? DfeSignInUserId { get; set; }
    }
}
