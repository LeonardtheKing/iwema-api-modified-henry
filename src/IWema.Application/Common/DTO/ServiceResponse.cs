namespace IWema.Application.Common.DTO;

public class ServiceResponse(string message, bool successful = false)
{
    public bool Successful { get; set; } = successful;
    public string Message { get; set; } = message;
}

public class ServiceResponse<T>(string message, bool successful = false, T response = default)
    : ServiceResponse(message, successful)
{
    public T Response { get; set; } = response;
}