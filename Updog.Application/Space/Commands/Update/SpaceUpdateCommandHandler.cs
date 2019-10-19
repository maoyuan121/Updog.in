using System.Threading.Tasks;
using Updog.Domain;

namespace Updog.Application {
    public sealed class SpaceUpdateCommandHandler : CommandHandler<SpaceUpdateCommand> {
        #region Fields
        private ISpaceService service;
        #endregion

        #region Constructor(s)
        public SpaceUpdateCommandHandler(ISpaceService service) {
            this.service = service;
        }
        #endregion

        #region Publics
        [Validate(typeof(SpaceUpdateCommandValidator))]
        [Policy(typeof(SpaceAlterCommandPolicy))]
        protected async override Task<Either<CommandResult, Error>> ExecuteCommand(SpaceUpdateCommand command) {
            try {
                Space s = await service.Update(command.Space, command.Update, command.User);
                return Success();
            } catch (NotFoundException e) {
                return Failure(e.Message);
            }
        }
        #endregion
    }
}