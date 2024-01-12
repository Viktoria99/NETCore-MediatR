using Ddd.Example.Service.Domain.Users.V10;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Application.Users.V10
{

    public class GetUserHandler : IRequestHandler<GetUserRequest, User>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserHandler"/> class.
        /// </summary>
        /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"><see cref="GetUserRequest"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>Task of<see cref="User"/>.</returns>
        public async Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserAsync(request.UserId);
        }
    }
}
