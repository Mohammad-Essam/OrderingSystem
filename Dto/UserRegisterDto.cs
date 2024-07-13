public class UserRegisterDTO
{
    public string? Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IList<string>? Roles { get; set; } = new List<string> { "user" };
}