using MongoDB.Driver;
using CommentsAPI.Models;
using Microsoft.Extensions.Options;
using CommentsAPI.Model;
using CommentsAPI.Interface;

namespace CommentsAPI.Services
{
    public class CommentToCommentService : ICommentToCommentService
    {
        IMongoCollection<CommentToComment> _comments;
        ICommentService _commentService;

        public CommentToCommentService(IOptions<LinkFlowCommentsDatabaseSettings> options, ICommentService commentService) 
        {
            _commentService = commentService;
            
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            _comments = database.GetCollection<CommentToComment>(options.Value.CommentsToCommentsCollectionName);
        }

        public async Task<CommentToComment> AddCommentToComment(string commentId, CommentToComment newComment)
        {
            Comment comments = await _commentService.GetCommentById(commentId);
            if (comments == null)
            {
                CommentToComment CmmtTComment = await GetCommentOfCommentFromId(commentId);
                
                if(CmmtTComment == null)
                {
                    throw new Exception("Comment not found");
                }

                await _comments.InsertOneAsync(newComment);
                if (string.IsNullOrEmpty(newComment.Id))
                {
                    throw new Exception("Failed to create new comment");
                }

                await InsertCommentToComment(commentId, newComment.Id);
                return newComment;
            }

            await _comments.InsertOneAsync(newComment);
            if (string.IsNullOrEmpty(newComment.Id))
            {
                throw new Exception("Failed to create new comment");
            }

            await _commentService.AddCommentToComment(commentId, newComment.Id);
            return newComment;
        }

        public async Task<CommentToComment> InsertCommentToComment(string commentId, string newCommentId)
        {
            CommentToComment comment = await GetCommentOfCommentFromId(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            comment.CommentsId.Add(newCommentId);
            await _comments.ReplaceOneAsync(comment => comment.Id == commentId, comment);

            return comment;
        }

        public async Task<CommentToComment> GetCommentOfCommentFromId(string commentId)
        {
            var sort = Builders<CommentToComment>.Sort.Descending(comment => comment.TimeStamp);
            return await _comments.Find(comment => comment.Id == commentId).FirstOrDefaultAsync();
        }

        public async Task<List<CommentToComment>> GetCommentsToCommentsFromUserId(string userId)
        {
            var sort = Builders<CommentToComment>.Sort.Descending(comment => comment.TimeStamp);
            return await _comments.Find(comment => comment.OwnerId == userId).ToListAsync();
        }

        public async Task<CommentToComment> UpdateCommentOfCommentText(string commentId, string text)
        {
            CommentToComment comment = await GetCommentOfCommentFromId(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            comment.Text = text;
            await _comments.ReplaceOneAsync(comment => comment.Id == commentId, comment);
            return comment;
        }

        public async Task<bool> DeleteComment(string commentId, string ownerId)
        {
            CommentToComment comment = await GetCommentOfCommentFromId(commentId);
            if (comment == null)
            {
                throw new Exception("Couldn't find this Comment");
            }

            if (comment.OwnerId != ownerId)
            {
                throw new Exception("The UserId and CommentOwnerId is not the same");
            }
            await UpdateCommentOfCommentText(commentId, "");

            comment = await GetCommentOfCommentFromId(commentId);
            return comment.Text == "" ? true : false;
        }

    }
}
