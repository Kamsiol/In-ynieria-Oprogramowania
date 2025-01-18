using Rover.Domain;
using FluentValidation;

namespace Rover.Application.UserInfo
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required");
            RuleFor(x => x.PhoneNunber)
                .GreaterThan(0)
                .WithMessage("Phone number must be greater than 0");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid Email is required");
            RuleFor(x => x.MainCity).NotEmpty().WithMessage("MainCity is required");
            RuleFor(x => x.Birthday)
                .NotEmpty()
                .WithMessage("Birthday must be a valid date in the past");
        }
    }
}
