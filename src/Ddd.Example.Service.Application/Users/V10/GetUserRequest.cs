using Ddd.Example.Service.Domain.Users.V10;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ddd.Example.Service.Application.Users.V10
{

    [ExcludeFromCodeCoverage]
    public class GetUserRequest : IRequest<User>
    {

        public int UserId { get; set; }
    }
}
