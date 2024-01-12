using Ddd.Example.Service.Application.Users.V10;
using FluentValidation;

namespace Ddd.Example.Service.Api.Validators.V10
{

    public class RequestModelValidator : AbstractValidator<GetUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestModelValidator"/> class.
        /// </summary>
        public RequestModelValidator()
        {
            RuleFor(query => query.UserId)
                .NotNull()
                .WithMessage("Not id")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id more then 0");
        }
    }
}
