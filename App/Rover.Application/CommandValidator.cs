using FluentValidation;
using Rover.Application.UserInfo;

public class CommandValidator : AbstractValidator<UserModify.Command>
{
    public CommandValidator()
    {
        RuleFor(x => x.User).SetValidator(new UserValidator());
    }
}