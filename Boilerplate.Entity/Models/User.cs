using Boilerplate.Entity.RequestModels.User;
using System;
using System.Text.Json.Serialization;

namespace Boilerplate.Entity.Models
{
    public class User : BaseModel
    {
        public User(CreateUserRequest request, Tuple<string, string> hashTuple)
        {
            Email = request.Email;
            Name = request.Name;
            Surname = request.Surname;
            Salt = hashTuple.Item1;
            HashedPassword = hashTuple.Item2;
        }

        public User()
        {
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
    }
}
