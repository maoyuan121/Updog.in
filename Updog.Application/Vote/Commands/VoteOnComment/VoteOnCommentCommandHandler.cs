using System.Threading.Tasks;
using Updog.Domain;

namespace Updog.Application {
    public sealed class VoteOnCommentCommandHandler : CommandHandler<VoteOnCommentCommand> {
        #region Fields
        private IVoteService service;
        #endregion

        #region Constructor(s)
        public VoteOnCommentCommandHandler(IVoteService service) {
            this.service = service;
        }
        #endregion

        #region Private
        [Validate(typeof(VoteOnCommentCommandValidator))]
        protected async override Task<CommandResult> ExecuteCommand(VoteOnCommentCommand command) {
            Vote v = await service.VoteOnComment(new VoteOnCommentData(command.CommentId, command.Vote), command.User);
            return new InsertResult(true, v.Id);
        }
        #endregion
    }
}