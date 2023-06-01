using System.Text.Json.Serialization;

namespace Task.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string? Api_token { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
