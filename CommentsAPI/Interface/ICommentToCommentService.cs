using CommentsAPI.Model;
using CommentsAPI.Models;

namespace CommentsAPI.Interface
{
    public interface ICommentToCommentService
    {
        public Task<CommentToComment> AddCommentToComment(string commentId, CommentToComment newComment);

        public Task<CommentToComment> InsertCommentToComment(string commentId, string newCommentId);

        public Task<CommentToComment> GetCommentOfCommentFromId(string commentId);

        public Task<List<CommentToComment>> GetCommentsToCommentsFromUserId(string userId);

        public Task<CommentToComment> UpdateCommentOfCommentText(string commentId, string text);

        public Task<bool> DeleteComment(string commentId, string ownerId);
    }
}
