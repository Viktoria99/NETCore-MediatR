using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Clients
{
    /// <summary>
    /// Import clients
    /// </summary>

    [ApiController]
    [ApiVersion("10")]
    [Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
    [Route("importClientFromEq{version:apiVersion}")]
    [ServiceFilter(typeof(ApiActionFilter))]
    public class ImportClientController : ControllerBase
    {

        /// <returns><see cref="Task"/></returns>
        [HttpPost(nameof(Import))]
        [SwaggerOperation(
            OperationId = nameof(Import),
            Summary = "Import client")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(void))]
        public async Task Import()
            => await Task.CompletedTask;
    }
}