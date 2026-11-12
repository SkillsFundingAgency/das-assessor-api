using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit4;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.Assessor.Data;
using SFA.DAS.Assessor.Domain.Configuration;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Application.UnitTests
{
    public static class AutofixtureExtensions
    {
        public static IFixture AssessorFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new AssessorCustomization());
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public static IDateTimeProvider? DateTimeProvider { get; set; }

        public AutoMoqDataAttribute()
            : base(AutofixtureExtensions.AssessorFixture)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoMoqInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineAutoDataAttribute(params object[] arguments)
            : base(() => AutofixtureExtensions.AssessorFixture(), arguments)
        {
        }
    }

    public class AssessorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new ApplicationSettingsBuilder());
            fixture.Customizations.Add(new AssessorDataContextBuilder());
            fixture.Customizations.Add(new DbContextOptionsBuilder());
        }
    }

    public class AssessorDataContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(AssessorDataContext))
            {
                var applicationSettings = context.Resolve(typeof(IOptions<ApplicationSettings>)) as IOptions<ApplicationSettings>;
                var dbContextOptions = context.Resolve(typeof(DbContextOptions<AssessorDataContext>)) as DbContextOptions<AssessorDataContext>;

                if (dbContextOptions != null)
                {
                    return GetAssessorDataContext(applicationSettings, dbContextOptions);
                }
            }

            return new NoSpecimen();
        }

        public static AssessorDataContext GetAssessorDataContext(
            IOptions<ApplicationSettings>? applicationSettings,
            DbContextOptions<AssessorDataContext> dbContextOptions)
        {
            var dbContext = new AssessorDataContext(
                applicationSettings,
                dbContextOptions);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }

    public class DbContextOptionsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DbContextOptions<AssessorDataContext>))
            {
                var connection = new SqliteConnection("Filename=:memory:");
                connection.Open();

                // These options will be used by the context instances in this test suite, including the connection opened above.
                var contextOptions = new DbContextOptionsBuilder<AssessorDataContext>()
                    .UseSqlite(connection)
                    .Options;

                return contextOptions;
            }

            return new NoSpecimen();
        }
    }

    public class ApplicationSettingsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(IOptions<ApplicationSettings>))
            {
                var applicationSettings = new ApplicationSettings
                {
                };

                return Options.Create(applicationSettings);
            }

            return new NoSpecimen();
        }
    }
}
