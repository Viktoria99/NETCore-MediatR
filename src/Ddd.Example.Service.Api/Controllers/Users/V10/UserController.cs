using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Ddd.Example.Service.Application.Users.V10;
using Ddd.Example.Service.Domain.Users.V10;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Users.V10
{

    [ApiController]
    [ApiVersion("10")]
    [Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
    [Route("user/v{version:apiVersion}")]
    [ServiceFilter(typeof(ApiActionFilter))]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IMediator"/>.</param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create exception
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="User"/>. </returns>
        [HttpPost(nameof(FindUserAsync))]
        [SwaggerOperation(
            OperationId = nameof(FindUserAsync),
            Summary = "Create exception")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Exception")]
        public async Task<User> FindUserAsync(
            [FromBody, SwaggerParameter("Enter any text")]FindUserRequest request)
            => await _mediator.Send(request);

        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="User"/></returns>
        [HttpPost(nameof(GetUserAsync))]
        [SwaggerOperation(
            OperationId = nameof(GetUserAsync),
            Summary = "Get user information")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
        public async Task<User> GetUserAsync(
            [FromBody, SwaggerParameter("Enter id")]GetUserRequest request)
            => await _mediator.Send(request);
    }
}
