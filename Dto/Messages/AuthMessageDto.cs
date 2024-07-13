using Task.Models;

namespace Task.Dto.Messages
{
    public class AuthMessageDto
    {
        public bool Status { get; set; }
        public User? User { get; set; }
        public string? token { get; set; }
        public string? ErrorMessage { get; internal set; }
    }
}
