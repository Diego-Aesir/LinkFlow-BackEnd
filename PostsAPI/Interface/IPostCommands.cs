using PostsAPI.DTO;
using PostsAPI.Models;

namespace PostsAPI.Interface
{
    public interface IPostCommands
    {
        public Task<Posts> CreatePost(Posts post);

        public Task<Posts> GetPostAsync(string postId);

        public Task<List<Posts>> GetRecentPosts(int page, int limit);

        public Task<List<Posts>> GetPostsFromTags(List<string> tags, int page, int limit);

        public Task<List<Posts>> GetPostsAsyncByOwner(string ownerId, int page, int limit);

        public Task<List<Posts>> GetPostsFromTitle(List<string> titles, int page, int limit);

        public Task<Posts> UpdatePostAsync(string id, string ownerId, PostsDTO updatedPost);

        public Task<bool> DeletePostAsync(string postId, string ownerId);
    }
}
