using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Ddd.Example.Service.Application.Reports.V10;
using Ddd.Example.Service.Domain.Reports.V10;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Reports.V10
{

    [ApiController]
    [ApiVersion("10")]
    [Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
    [Route("report/v{version:apiVersion}")]
    [ServiceFilter(typeof(ApiActionFilter))]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IMediator"/>.</param>
        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Receive info from database
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="Report"/></returns>
        [HttpPost(nameof(GetReportAsync))]
        [SwaggerOperation(
            OperationId = nameof(GetReportAsync),
            Summary = "Receive info")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Report))]
        public async Task<Report> GetReportAsync(
            [FromBody, SwaggerParameter("Enter id")]ReportRequest request)
            => await _mediator.Send(request);
    }
}
