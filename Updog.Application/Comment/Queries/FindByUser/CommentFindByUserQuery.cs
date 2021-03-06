using Updog.Domain;
using Updog.Domain.Paging;

namespace Updog.Application {
    public sealed class CommentFindByUserQuery : AnonymousQuery, IPagableQuery {
        #region Properties
        public string Username { get; }
        public PaginationInfo Paging { get; }
        #endregion

        #region Constructor(s)
        public CommentFindByUserQuery(string username, PaginationInfo? paging = null, User? user = null) : base(user) {
            Username = username;
            Paging = paging ?? new PaginationInfo(0, Comment.PageSize);
        }
        #endregion
    }
}