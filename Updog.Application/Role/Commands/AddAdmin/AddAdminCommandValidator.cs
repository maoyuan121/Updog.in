using FluentValidation;

namespace Updog.Application {
    public sealed class AddAdminCommandValidator : FluentValidatorAdapter<AddAdminCommand> {
        public AddAdminCommandValidator() {
            RuleFor(c => c.Username).NotNull().WithMessage("Username is required.");
            RuleFor(c => c.Username).NotEmpty().WithMessage("Username is required.");
        }
    }
}