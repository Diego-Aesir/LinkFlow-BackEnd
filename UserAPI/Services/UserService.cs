using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserAPI.DTO;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<LinkFlowUsersDatabaseSettings> settings, IConfiguration config)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(settings.Value.UsersCollectionName);
        
            var indexKeys = Builders<User>.IndexKeys.Ascending(user => user.UserName);
            var indexOptions = new CreateIndexOptions { Unique = true };
            _usersCollection.Indexes.CreateOne(new CreateIndexModel<User>(indexKeys, indexOptions));
        }

        public async Task<User?> CreateUserAsync(User newUser)
        {
            try
            {
                if (!string.IsNullOrEmpty(newUser.Password))
                {
                    newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                }
                await _usersCollection.InsertOneAsync(newUser);
                return await GetUserAsyncByUsername(newUser.UserName);
            }
            catch (MongoWriteException ex) when (ex.WriteError?.Code == 11000)
            {
                throw new Exception("Username already exists, please choose a different one.");
            }
        }

        public async Task<User?> GetUserAsyncByUsername(string username)
        {
            return await _usersCollection.Find(user => user.UserName == username).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(string id, UpdateUser updatedUser)
        {
            User foundUser = await GetUserAsync(id);
            if (foundUser == null) 
            {
                throw new Exception("User not found");
            }
            User userCopy = new User
            {
                Id = id,
                UserName = string.IsNullOrEmpty(updatedUser.UserName) ? foundUser.UserName : updatedUser.UserName,
                UserEmail = string.IsNullOrEmpty(updatedUser.UserEmail) ? foundUser.UserEmail : updatedUser.UserEmail,
                Name = string.IsNullOrEmpty(updatedUser.Name) ? foundUser.Name : updatedUser.Name,
                Pronoun = string.IsNullOrEmpty(updatedUser.Pronoun) ? foundUser.Pronoun : updatedUser.Pronoun,
                Password = string.IsNullOrEmpty(updatedUser.Password) ? foundUser.Password : BCrypt.Net.BCrypt.HashPassword(updatedUser.Password),
                Profile = string.IsNullOrEmpty(updatedUser.Profile) ? foundUser.Profile : updatedUser.Profile,
                Gender = string.IsNullOrEmpty(updatedUser.Gender) ? foundUser.Gender : updatedUser.Gender,
                Photo = string.IsNullOrEmpty(updatedUser.Photo) ? foundUser.Photo : updatedUser.Photo
            };
            await _usersCollection.ReplaceOneAsync(x =>  x.Id == id, userCopy);
            return await GetUserAsync(id);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(user => user.Id == id);
            var user = await GetUserAsync(id);
            return user == null ? true : false;
        }
    }
}
