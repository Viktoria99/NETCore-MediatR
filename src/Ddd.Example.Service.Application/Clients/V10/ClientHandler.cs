using Ddd.Example.Service.Domain.Clients.V10;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Application.Clients.V10
{

    public class ClientHandler : IRequestHandler<ClientRequest, ExtendedClient>
    {
        private readonly IClientODataService _clientODataService;
        private readonly IClientService _clientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="clientODataService"><see cref="IClientODataService"/>.</param>
        /// <param name="clientService"><see cref="IClientService"/>.</param>
        public ClientHandler(IClientODataService clientODataService, IClientService clientService)
        {
            _clientODataService = clientODataService;
            _clientService = clientService;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="request"><see cref="ClientRequest"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Task of <see cref="ExtendedClient"/>.</returns>
        public async Task<ExtendedClient> Handle(ClientRequest request, CancellationToken cancellationToken)
        {
            var clientId = request.ClientId;

            var clientTask = _clientODataService.GetClientAsync(clientId);
            var segmentTask = _clientService.GetSegmentAsync(clientId);

            await Task.WhenAll(clientTask, segmentTask);

            return new ExtendedClient
            {
                Client = clientTask,
                Segment = segmentTask
            };
        }
    }
}
