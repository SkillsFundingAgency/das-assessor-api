using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Assessor.Domain.Entities;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Data
{
    public class UserEntityContext(AssessorDataContext context) : IUserEntityContext
    {
        public Task<User?> GetFirstOrDefault() =>
            context.Users.FirstOrDefaultAsync();

        public Task<User?> Get(string dfeSignInUserId) =>
            context.Users.FirstOrDefaultAsync(user => user.DfeSignInUserId == dfeSignInUserId);

        public Task<User?> GetByUserId(Guid userId) =>
            context.Users.FirstOrDefaultAsync(user => user.Id == userId);

        public async Task<bool> TryCreateUserAsync(
            User user,
            CancellationToken cancellationToken)
        {
            var entry = context.Users.Add(user);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is SqlException { Number: 2601 } sqlException &&
                sqlException.Message.Contains(
                    "'UX_User_DfeSignInUserId'", StringComparison.Ordinal))
            {
                entry.State = EntityState.Detached;
                return false;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            context.SaveChangesAsync(cancellationToken);
    }
}