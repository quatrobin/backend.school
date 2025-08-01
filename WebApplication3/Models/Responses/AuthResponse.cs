namespace WebApplication3.Models.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserProfileResponse User { get; set; } = new UserProfileResponse();
} 