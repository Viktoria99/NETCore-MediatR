using Ddd.Example.Service.Domain.Clients.V10;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Infrastructure.Services.Clients.V10.REST
{
    public partial class ClientService : IClientService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService"/> class.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/></param>
        /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/>.</param>
        public ClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            BaseUrl = httpClient.BaseAddress.ToString();
            _httpClient = httpClient;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() => new Newtonsoft.Json.JsonSerializerSettings());

            _httpClient.DefaultRequestHeaders.Add("REQUEST-ID", httpContextAccessor.HttpContext.Request.Headers["REQUEST-ID"].FirstOrDefault());
        }


        public async Task<Segment> GetSegmentAsync(int clientId)
        {
            var segmentInfo = await GetSegmentAsync(clientId, "1.0");

            return segmentInfo == null ? default(Segment) : new Segment
            {
                IdClient = segmentInfo.IdClient,
                SegmentCode = segmentInfo.SegmentCode
            };
        }
    }
}
