using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ddd.Example.Service.Application.Clients.V10
{

    [ExcludeFromCodeCoverage]
    public class ClientRequest : IRequest<ExtendedClient>
    {

        public int ClientId { get; set; }
    }
}
