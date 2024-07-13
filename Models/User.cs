using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Task.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public string? Api_token { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
