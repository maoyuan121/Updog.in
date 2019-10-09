using FluentValidation;
using Updog.Application.Validation;
using Updog.Domain;

namespace Updog.Application {
    internal sealed class VoteOnCommentCommandValidator : FluentValidatorAdapter<VoteOnCommentCommand> {
        #region Constructor(s)
        public VoteOnCommentCommandValidator() {
            // RuleFor(p => p.CommentId).GreaterThan(0).WithMessage("Comment Id is required.");
            // RuleFor(p => p.Vote).IsInEnum().WithMessage("Vote direction must be 1, 0, or -1");
            RuleFor(p => p.User).NotNull().WithMessage("User is required.");
        }
        #endregion
    }
}