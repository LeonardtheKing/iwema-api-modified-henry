using IWema.Application.Common.DTO;
using IWema.Application.Contract.Login;
using MediatR;

namespace IWema.Application.Login.Command;

public class LoginCommand : IRequest<ServiceResponse<LoginResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
