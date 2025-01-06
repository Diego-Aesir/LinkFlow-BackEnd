using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using LinkFlowWebBFF.DTO;
using Microsoft.IdentityModel.Tokens;

namespace LinkFlowWebBFF.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        const string UserBaseUrl = "http://localhost:8090";
        const string PostsBaseUrl = "http://localhost:9080";
        const string CommentsBaseUrl = "http://localhost:9090";
        IConfiguration _configuration;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _configuration = config;
        }

        public async Task<UserResponse> CreateUser(User user)
        {
            if (user.Password == null) 
            {
                throw new Exception("User needs a password");
            }

            return await CreateUserWithGoogle(user);
        }

        public async Task<UserResponse> CreateUserWithGoogle(User user)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{UserBaseUrl}/api/User/register", jsonBody);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var jsonResponse = await GetUserByUsername(user.UserName);
                var userResponse = JsonSerializer.Deserialize<UserIdAndUsername>(jsonResponse); 
                UserIdAndUsername idAndUsername = new() { id = userResponse.id, userName = userResponse.userName };
                return JWT(idAndUsername);
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetUserById(string userId)
        {
            var response = await _httpClient.GetAsync($"{UserBaseUrl}/api/User/id:{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error getting user: {response.StatusCode} - {errorDetails}");
            }
        }

        private async Task<string> GetUserByUsername(string username)
        {
            var response = await _httpClient.GetAsync($"{UserBaseUrl}/api/User/username:{username}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error getting user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> UpdateUser(string userId, UpdateUser updatedUser)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(updatedUser), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{UserBaseUrl}/api/User/update/{userId}", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> DeleteUser(string userId)
        {
            var response = await _httpClient.DeleteAsync($"{UserBaseUrl}/api/User/delete/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<UserResponse> LoginUser(LoginUser user)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{UserBaseUrl}/api/User/login", jsonBody);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserIdAndUsername>(jsonResponse);
                UserIdAndUsername userIdAndUsername = new() { id = userInfo.id, userName = userInfo.userName };

                return JWT(userIdAndUsername);
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while loging user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<UserResponse> LoginUserWithGoogle(LoginUser user)
        {
            user.Password = null;

            var jsonBody = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{UserBaseUrl}/api/User/login/google", jsonBody);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserIdAndUsername>(jsonResponse);
                UserIdAndUsername userIdAndUsername = new() { id = userInfo.id, userName = userInfo.userName };

                return JWT(userIdAndUsername);
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while loging user: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> CreatePost(Posts post)
        {
            try
            {
                var jsonBody = new StringContent(
                                JsonSerializer.Serialize(post),
                                Encoding.UTF8,
                                "application/json");
                var response = await _httpClient.PostAsync($"{PostsBaseUrl}/api/Posts/createPost", jsonBody);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> GetPost(string postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{PostsBaseUrl}/api/Posts/{postId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> GetRecentPostsWithoutTag(int page, int limit)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{PostsBaseUrl}/api/Posts/recentPosts/{page}/{limit}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
               throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> GetRecentPostsWithTags(int page, int limit, List<string> tags)
        {
            try
            {
                var tagsQuery = string.Join(",", tags);
                var response = await _httpClient.GetAsync($"{PostsBaseUrl}/api/Posts/recentPosts/FromTags/{page}/{limit}?tags={tagsQuery}");
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                } else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> GetRecentPostsFromUser(int page, int limit, string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{PostsBaseUrl}/api/Posts/recentPosts/FromUser:{userId}/{page}/{limit}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> GetRecentPostsFromTitle(int page, int limit, List<string> titles)
        {
            try
            {
                var titlesQuery = string.Join(",", titles);
                var response = await _httpClient.GetAsync($"{PostsBaseUrl}/api/Posts/recentPosts/FromTitle/{page}/{limit}?titles={titlesQuery}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> UpdatePost(string ownerId, string postId, Posts post)
        {
            try
            {
                var jsonBody = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{PostsBaseUrl}/api/Posts/updatePost/{ownerId}/{postId}", jsonBody);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }

        public async Task<string> DeletePost(string postId, string userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{PostsBaseUrl}/api/Posts/deletePost/{userId}/{postId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating post: {response.StatusCode} - {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while making http call.", ex);
            }
        }
    
        public async Task<string> CreateComment(Comments comment)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(comment), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{CommentsBaseUrl}/api/Comment/CreateComment",  jsonBody);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating Comment: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetCommentById(string commentId)
        {
            var response = await _httpClient.GetAsync($"{CommentsBaseUrl}/api/Comment/GetCommentFromCommentId:{commentId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error getting Comment by Id: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetCommentByPostId(string postId)
        {
            var response = await _httpClient.GetAsync($"{CommentsBaseUrl}/api/Comment/GetCommentsIdFromPostId:{postId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error getting comment by Post id: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetCommentByUserId(string userId)
        {
            var response = await _httpClient.GetAsync($"{CommentsBaseUrl}/api/Comment/GetCommentsIdFromUser:{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error on getting post: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> UpdateCommentText(string commentId, string commentText)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(commentText), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{CommentsBaseUrl}/api/Comment/UpdateCommentText:{commentId}", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error updating Comment Text: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> DeleteCommentTextById(string commentId, string ownerId)
        {
            var response = await _httpClient.DeleteAsync($"{CommentsBaseUrl}/api/Comment/Owner:{ownerId}/DeleteCommentText:{commentId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while deleting Comment text: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> createCommentToComment(string commentParentId, CommentsToComments commentsToComments)
        {
            commentsToComments.CommentParentId = commentParentId;
            var jsonBody = new StringContent(JsonSerializer.Serialize(commentsToComments), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{CommentsBaseUrl}/api/CommentToComment/CreateCommentToComment:{commentParentId}", jsonBody);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while Creating Comment to Comment: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetCommentToCommentFromId(string commentId)
        {
            var response = await _httpClient.GetAsync($"{CommentsBaseUrl}/api/CommentToComment/GetCommentOfComment:{commentId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while Getting Comment to Comment by Id: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> GetCommentToCommentsFromUserId(string userId)
        {
            var response = await _httpClient.GetAsync($"{CommentsBaseUrl}/api/CommentToComment/GetCommentOfCommentFromUserId:{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while Getting Comment to Comment by User Id: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> UpdateCommentOfCommentText(string commentId, string text)
        {
            var jsonBody = new StringContent(JsonSerializer.Serialize(text), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{CommentsBaseUrl}/api/CommentToComment/UpdateCommentText:{commentId}", jsonBody);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while Updating Comment to Comment Text: {response.StatusCode} - {errorDetails}");
            }
        }

        public async Task<string> DeleteCommentToCommentTextById(string commentId, string ownerId)
        {
            var response = await _httpClient.DeleteAsync($"{CommentsBaseUrl}/api/CommentToComment/Owner:{ownerId}/DeleteCommentText:{commentId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error while Deleting Comment to Comment Text: {response.StatusCode} - {errorDetails}");
            }
        }
    
        private UserResponse JWT (UserIdAndUsername user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id),
                new Claim(ClaimTypes.Name, user.userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credential);

            UserResponse response = new() 
            {
                Id = user.id,
                Jwt = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return response;
        }
    }
}