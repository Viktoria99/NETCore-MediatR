using Ddd.Example.Service.Api.Controllers.Users.V10;
using Ddd.Example.Service.Application.Users.V10;
using Ddd.Example.Service.Domain.Users.V10;
using MediatR;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ddd.Example.Service.Api.Tests.Unit.Controllers.Users.V10
{

    [ExcludeFromCodeCoverage]
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTests"/> class.
        /// </summary>
        public UserControllerTests()
        {
            _mediator = new Mock<IMediator>();
        }


        [Fact]
        public void GenerateErrorAsync_Success()
        {
            const string name = "name";
            var error = $"Error: {name}.";

            var dto = new FindUserRequest
            {
                Name = name
            };

            _mediator.Setup(x => x.Send(dto, CancellationToken.None)).Returns(Task.FromException<User>(new Exception(error)));
            var controller = new UserController(_mediator.Object);

            var result = controller.FindUserAsync(dto).Exception.Message;

            Assert.Equal($"One or more errors occurred. ({error})", result);
        }


        [Fact]
        public async Task GetUserAsync_Success()
        {
            var id = 10;
            var dto = new GetUserRequest
            {
                UserId = id
            };

            var user = new User
            {
                Id = id,
                Login = "login",
                ProfileEQ = "profileEQ",
            };

            _mediator.Setup(x => x.Send(dto, CancellationToken.None)).Returns(Task.FromResult(user));
            var controller = new UserController(_mediator.Object);

            var result = await controller.GetUserAsync(dto);

            Assert.Equal(user, result);
        }
    }
}
