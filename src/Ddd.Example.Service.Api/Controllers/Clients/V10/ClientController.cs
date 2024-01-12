using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Ddd.Example.Service.Application.Clients.V10;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Clients.V10
{

    [ApiVersion("10")]
    [ApiController]
    [Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
    [Route("client/v{version:apiVersion}")]
    [ServiceFilter(typeof(ApiActionFilter))]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IMediator"/>.</param>
        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Receive information about clients.
        /// </summary>
        /// <param name="query"><see cref="ClientRequest"/>>.</param>
        /// <returns><see cref="ExtendedClient"/></returns>
        [HttpGet(nameof(GetClientAsync))]
        [SwaggerOperation(
            OperationId = nameof(GetClientAsync),
            Summary = "Get information")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ExtendedClient))]
        public async Task<ExtendedClient> GetClientAsync([FromQuery]ClientRequest query)
            => await _mediator.Send(query);
    }
}
