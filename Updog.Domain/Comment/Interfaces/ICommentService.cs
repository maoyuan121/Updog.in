using System.Threading.Tasks;

namespace Updog.Domain {
    public interface ICommentService : IService<Comment> {
        Task<Comment?> FindById(int id);
        Task<Comment> Create(CommentCreate create, User user);
        Task<Comment> Update(int commentId, CommentUpdate update, User user);
        Task<Comment> Delete(int commentId, User user);
        Task<bool> IsOwner(int commentId, string username);
    }
}