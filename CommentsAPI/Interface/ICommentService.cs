using CommentsAPI.Model;
using CommentsAPI.Models;

namespace CommentsAPI.Interface
{
    public interface ICommentService
    {
        public Task<Comment> CreateComment(Comment newComment);

        public Task<Comment> GetCommentById(string commentId);

        public Task<List<Comment>> GetCommentsFromPost(string postId);

        public Task<List<Comment>> GetCommentsFromUser(string userId);

        public Task<Comment> UpdateCommentText(string commentId, string text);

        public Task<bool> DeleteComment(string commentId, string ownerId);

        public Task<Comment> AddCommentToComment(string commentId, string newCommentId);
    }
}
