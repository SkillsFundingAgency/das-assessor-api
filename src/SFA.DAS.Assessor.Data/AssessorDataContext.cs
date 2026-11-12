using System.Diagnostics.CodeAnalysis;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.Assessor.Data.Configuration;
using SFA.DAS.Assessor.Domain.Configuration;
using SFA.DAS.Assessor.Domain.Entities;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Data
{
    [ExcludeFromCodeCoverage]
    public class AssessorDataContext : DbContext,
        IUserEntityContext
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly ApplicationSettings? _configuration;
        private readonly ChainedTokenCredential? _chainedTokenCredentialProvider;

        public virtual DbSet<User> Users { get; set; }
        
        DbSet<User> IEntityContext<User>.Entities => Users;
        
        public AssessorDataContext(IOptions<ApplicationSettings>? config,
            DbContextOptions<AssessorDataContext> options)
            : base(options)
        {
            _configuration = config?.Value;
        }

        public AssessorDataContext(IOptions<ApplicationSettings>? config,
            DbContextOptions<AssessorDataContext> options,
            ChainedTokenCredential chainedTokenCredentialProvider)
            : base(options)
        {
            _configuration = config?.Value;
            _chainedTokenCredentialProvider = chainedTokenCredentialProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_chainedTokenCredentialProvider != null)
            {
                var connection = new SqlConnection
                {
                    ConnectionString = _configuration?.DbConnectionString,
                    AccessToken = _chainedTokenCredentialProvider
                        .GetTokenAsync(new TokenRequestContext(scopes: [AzureResource]))
                        .GetAwaiter().GetResult().Token
                };

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
