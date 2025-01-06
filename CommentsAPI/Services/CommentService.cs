using CommentsAPI.Interface;
using CommentsAPI.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsAPI.Services
{
    public class CommentService : ICommentService
    {
        IMongoCollection<Comment> _comments;

        public CommentService(IOptions<LinkFlowCommentsDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            _comments = database.GetCollection<Comment>(options.Value.CommentsCollectionName);
        }

        public async Task<Comment> CreateComment(Comment newComment)
        {
            await _comments.InsertOneAsync(newComment);
            return newComment;
        }

        public async Task<Comment> GetCommentById(string commentId)
        {
            return await _comments.Find(comment =>  comment.Id == commentId).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetCommentsFromPost(string postId)
        {
            var sort = Builders<Comment>.Sort.Descending(comment => comment.TimeStamp);
            return await _comments.Find(comments =>  comments.PostId == postId).Sort(sort).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsFromUser(string userId)
        {
            var sort = Builders<Comment>.Sort.Descending(comment => comment.TimeStamp);
            return await _comments.Find(comments => comments.OwnerId == userId).Sort(sort).ToListAsync();
        }

        public async Task<Comment> UpdateCommentText(string commentId, string text)
        {
            Comment comment = await GetCommentById(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            comment.Text = text;
            await _comments.ReplaceOneAsync(comment => comment.Id == commentId, comment);
            return comment;
        }

        public async Task<Comment> AddCommentToComment(string commentId, string newCommentId)
        {
            Comment comment = await GetCommentById(commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }

            comment.CommentsId.Add(newCommentId);
            await _comments.ReplaceOneAsync(comment => comment.Id == commentId, comment);

            return comment;
        }

        public async Task<bool> DeleteComment(string commentId, string ownerId)
        {
            Comment comment = await GetCommentById(commentId);
            if (comment == null)
            {
                throw new Exception("Couldn't find this Comment");
            }

            if (comment.OwnerId != ownerId) 
            {
                throw new Exception("The UserId and CommentOwnerId is not the same");
            }


            await UpdateCommentText(commentId, "");
            var commentExist = await GetCommentById(commentId);
            
            return commentExist.Text == "" ? true : false;
        }
    }
}
