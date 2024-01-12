using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Ddd.Example.Service.Api.Logging
{

    public class AdditionalLogState
    {

        public IDictionary<string, object> Create(HttpContext httpContext) =>
            new Dictionary<string, object>
            {
                ["user_id"] = httpContext.User.Identity.Name,
                ["query"] = httpContext.Request.QueryString
            };
    }
}
