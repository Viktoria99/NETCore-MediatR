using Ddd.Example.Service.Api.Controllers.Filters.V10;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Api.Controllers.Users
{

    [ApiController]
    [ApiVersion("20")]
    [Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
    [Route("importUserFromAd{version:apiVersion}")]
    [ServiceFilter(typeof(ApiActionFilter))]
    public class UserInfo2Controller : ControllerBase
    {

        [HttpPost(nameof(Import))]
        [SwaggerOperation(
            OperationId = nameof(Import),
            Summary = "Import user")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(void))]
        public async Task Import()
            => await Task.CompletedTask;
    }
}