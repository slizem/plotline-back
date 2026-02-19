namespace Plotline.Core.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public User User { get; set; }
    }
}