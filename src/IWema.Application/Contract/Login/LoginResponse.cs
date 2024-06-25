namespace IWema.Application.Contract.Login;

public record LoginResponse(string Name, string Email, string JwtToken, string RefreshToken, string UserRole);

