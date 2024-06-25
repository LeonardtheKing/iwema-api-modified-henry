namespace IWema.Application.Contract.Login;

public class LoginOutputModel
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
