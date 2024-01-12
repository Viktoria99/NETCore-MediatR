using Ddd.Example.Service.Domain.Clients.V10;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Application.Clients.V10
{
    public class ExtendedClient
    {

        public Task<Client> Client { get; set; }

        public Task<Segment> Segment { get; set; }
    }
}
