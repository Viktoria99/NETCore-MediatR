using System.Threading.Tasks;

namespace Ddd.Example.Service.Domain.Clients.V10
{

    public interface IClientService
    {

        Task<Segment> GetSegmentAsync(int clientId);
    }
}
