namespace UserAPI.DTO
{
    public class UserLogin
    {
        public required string UserName {  get; set; }

        public string? Password { get; set; }
    }
}
