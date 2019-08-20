using System.Threading.Tasks;
using Updog.Domain;

namespace Updog.Application {
    /// <summary>
    /// Interactor to find a comment by it's ID.
    /// </summary>
    public sealed class CommentFinderById : IInteractor<int, CommentView> {
        #region Fields
        /// <summary>
        /// CRUD interface for managing comments in the database.
        /// </summary>
        private ICommentRepo commentRepo;

        /// <summary>
        /// Mapper to convert a comment into it's DTO.
        /// </summary>
        private IMapper<Comment, CommentView> commentMapper;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new comment finder that locates comments via ID.
        /// </summary>
        /// <param name="commentRepo">CRUD interface for comments in the database.</param>
        /// <param name="commentMapper">Mapper to build comment DTOs.</param>
        public CommentFinderById(ICommentRepo commentRepo, IMapper<Comment, CommentView> commentMapper) {
            this.commentRepo = commentRepo;
            this.commentMapper = commentMapper;
        }
        #endregion

        #region Publics
        public async Task<CommentView> Handle(int input) {
            Comment comment = await commentRepo.FindById(input);

            if (comment == null) {
                return null;
            }

            return commentMapper.Map(comment);
        }
        #endregion
    }
}