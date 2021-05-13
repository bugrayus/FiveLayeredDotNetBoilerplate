namespace Boilerplate.Entity.ResponseModels.User
{
    public class LoginResponse
    {
        public LoginResponse()
        {
        }

        public LoginResponse(Models.User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Token = token;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
