namespace Blasterify.Models.Requests
{
    public class LogInRequest
    {
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
    }
}