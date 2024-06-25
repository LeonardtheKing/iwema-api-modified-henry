namespace IWema.Application.Login.Command;


//public record LoginInputModel(string Email, string Password);

public class LoginInputModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}