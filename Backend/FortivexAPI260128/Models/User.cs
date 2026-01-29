using System.Security.Claims;
using System.Text.Json.Serialization;

namespace FortivexAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; }  = "User";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }

        public int AccountId { get; set; }

        internal static User? FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
