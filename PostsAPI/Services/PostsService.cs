using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PostsAPI.DTO;
using PostsAPI.Interface;
using PostsAPI.Models;

namespace PostsAPI.Services
{
    public class PostsService : IPostCommands
    {
        public IMongoCollection<Posts> _postsCollection { get; set; }
        
        public PostsService(IOptions<LinkFlowPostsDatabaseSettings> options) 
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _postsCollection = mongoDatabase.GetCollection<Posts>(options.Value.PostsCollectionName);
        }

        public async Task<Posts> CreatePost(Posts post)
        {
            await _postsCollection.InsertOneAsync(post);
            return post;
        }

        public async Task<Posts> GetPostAsync(string postId)
        {
            Posts found = await _postsCollection.Find(post => post.Id == postId).FirstOrDefaultAsync();
            return found;
        }

        public async Task<List<Posts>> GetRecentPosts(int page, int limit)
        {
            var skip = (page - 1) * limit;
            var filter = Builders<Posts>.Filter.Empty;
            var sort = Builders<Posts>.Sort.Descending(post => post.Date);

            return await _postsCollection.Find(filter)
                                         .Skip(skip)
                                         .Sort(sort)
                                         .Limit(limit)
                                         .ToListAsync();
        }

        public async Task<List<Posts>> GetPostsFromTags(List<string> tags, int page, int limit)
        {
            var skip = (page - 1) * limit;
            var filters = tags.Select(tag => Builders<Posts>.Filter.Regex(post => post.Tags, new BsonRegularExpression(tag, "i"))).ToList();
            var combinedFilter = Builders<Posts>.Filter.And(filters);
            return await _postsCollection.Find(combinedFilter)
                                         .Skip(skip)
                                         .Limit(limit)
                                         .ToListAsync();
        }

        public async Task<List<Posts>> GetPostsAsyncByOwner(string ownerId, int page, int limit)
        {
            var skip = (page - 1) * limit;
            List<Posts> list = await _postsCollection.Find(posts =>  posts.OwnerId == ownerId).Skip(skip).Limit(limit).ToListAsync();
            return list;
        }

        public async Task<List<Posts>> GetPostsFromTitle(List<string> titles, int  page, int limit)
        {
            var skip = (page - 1) * limit;
            var filters = titles.Select(t => Builders<Posts>.Filter.Regex(posts => posts.Title, new BsonRegularExpression(t, "i"))).ToList();
            var combinedFilter = Builders<Posts>.Filter.Or(filters);
            return await _postsCollection.Find(combinedFilter)
                                 .Skip(skip)
                                 .Limit(limit)
                                 .ToListAsync();
        }

        public async Task<Posts> UpdatePostAsync(string id, string ownerId, PostsDTO updatedPost)
        {
            Posts foundPost = await GetPostAsync(id);
            if(foundPost == null)
            {
                throw new Exception("Post not found");
            }

            if(foundPost.OwnerId != ownerId)
            {
                throw new Exception("Not the owner");
            }

            Posts postCopy = new Posts
            {
                Id = id,
                Title = string.IsNullOrEmpty(updatedPost.Title) ? foundPost.Title : updatedPost.Title,
                Photo = string.IsNullOrEmpty(updatedPost.Photo) ? foundPost.Photo : updatedPost.Photo,
                Text = string.IsNullOrEmpty(updatedPost.Text) ? foundPost.Text : updatedPost.Text,
                Tags = updatedPost.Tags != null && updatedPost.Tags.Count > 0 ? updatedPost.Tags : foundPost.Tags,
                Date = foundPost.Date,
                OwnerId = foundPost.OwnerId,
            };

            await _postsCollection.ReplaceOneAsync(post => post.Id == id, postCopy);
            return await GetPostAsync(id);
        }

        public async Task<bool> DeletePostAsync(string postId, string ownerId)
        {
            Posts post = await GetPostAsync(postId);
            if(post.OwnerId != ownerId)
            {
                return false;
            }

            await _postsCollection.DeleteOneAsync(post => post.Id == postId);
            var postExist = await GetPostAsync(postId);
            return postExist == null ? true : false;
        }
    }
}
