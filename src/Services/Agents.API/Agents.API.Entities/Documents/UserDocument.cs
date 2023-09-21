using Interfaces.Service;
using System.Text.Json.Serialization;

namespace Agents.API.Entities.Documents
{
    public class UserDocument : Document
    {
        public const string ADMIN = "Admin";
        public const string EDITOR = "Editor";

        public string Login { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsBan { get; set; }

        public void SetPassword(string pwd)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(pwd);
        }

        public bool CheckPassword(string pwd)
        {
            return BCrypt.Net.BCrypt.Verify(pwd, Password);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
