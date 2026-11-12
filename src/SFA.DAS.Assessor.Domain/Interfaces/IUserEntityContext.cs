using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Assessor.Domain.Entities;

namespace SFA.DAS.Assessor.Domain.Interfaces
{
    public interface IUserEntityContext : IEntityContext<User>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<User?> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();

        public async Task<User?> Get(string dfeSignInUserId)
            => await Entities
                .FirstOrDefaultAsync(er => er.DfeSignInUserId == dfeSignInUserId);

        public async Task<User?> GetByUserId(Guid userId)
            => await Entities
                .FirstOrDefaultAsync(er => er.Id == userId);
    }
}
