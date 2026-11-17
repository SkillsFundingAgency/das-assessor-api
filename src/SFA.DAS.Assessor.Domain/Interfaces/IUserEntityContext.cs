using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Assessor.Domain.Entities;

namespace SFA.DAS.Assessor.Domain.Interfaces
{
    public interface IUserEntityContext
    {
        Task<User?> GetFirstOrDefault();
        Task<User?> Get(string dfeSignInUserId);
        Task<User?> GetByUserId(Guid userId);
        Task<bool> TryCreateUserAsync(User user, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}