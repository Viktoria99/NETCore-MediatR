using Ddd.Example.Service.Domain.Users.V10;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Application.Users.V10
{

    public class FindUserHandler : IRequestHandler<FindUserRequest, User>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
        public FindUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(FindUserRequest request, CancellationToken cancellationToken)
        {
            return await _userRepository.FindUserAsync(request.Name);
        }
    }
}
