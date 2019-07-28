using System.Threading.Tasks;
using Updog.Domain;
using FluentValidation;

namespace Updog.Application {
    /// <summary>
    /// Interactor to handle deleting posts.
    /// </summary>
    public sealed class PostDeleter : IInteractor<PostDeleteParams, Post> {
        #region Fields
        private IPermissionHandler<Post> permissionHandler;

        private IPostRepo repo;

        private AbstractValidator<PostDeleteParams> validator;
        #endregion

        #region Constructor(s)
        public PostDeleter(IPermissionHandler<Post> permissionHandler, IPostRepo repo, AbstractValidator<PostDeleteParams> validator) {
            this.permissionHandler = permissionHandler;
            this.repo = repo;
            this.validator = validator;
        }
        #endregion

        #region Publics
        public async Task<Post> Handle(PostDeleteParams input) {
            Post p = await repo.FindById(input.PostId);

            if (p == null) {
                throw new NotFoundException();
            }

            if (!(await this.permissionHandler.HasPermission(input.User, PermissionAction.DeletePost, p))) {
                throw new AuthorizationException();
            }

            await validator.ValidateAndThrowAsync(input);

            p.WasDeleted = true;
            await repo.Update(p);
            return p;
        }
        #endregion
    }
}