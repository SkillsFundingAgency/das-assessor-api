using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessor.Api.Controllers;
using SFA.DAS.Assessor.Api.Models;
using SFA.DAS.Assessor.Application.Queries.GetUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessor.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUser
    {
        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsSuccessful_Then_ReturnOk
            (string dfeSignInUserId,
            [Frozen] Mock<IMediator> mediator,
            GetUserQueryResult userResult,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetUserQuery>(t => t.DfeSignInUserId == dfeSignInUserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userResult);

            // Act
            var result = await controller.GetUser(dfeSignInUserId);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo((GetUserResponse?)userResult.User);
        }

        [Test, MoqAutoData]
        public async Task And_ValidationFails_Then_ReturnBadRequestWithErrors
            (string govUkIdentifier,
            [Frozen] Mock<IMediator> mediator,
            ValidationException validationException,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .Throws(validationException);

            // Act
            var result = await controller.GetUser(govUkIdentifier);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeEquivalentTo(new { errors = validationException.Errors });
        }

        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_Return500Result(
            string govUkIdentifier,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.GetUser(govUkIdentifier);

            // Assert
            var statusCodeResult = result as StatusCodeResult;

            statusCodeResult.Should().NotBeNull();
            statusCodeResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
