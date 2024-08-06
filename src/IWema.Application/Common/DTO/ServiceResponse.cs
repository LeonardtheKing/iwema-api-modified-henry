namespace IWema.Application.Common.DTO;

public class ServiceResponse
{
    public bool Successful { get; set; }
    public string Message { get; set; }

    public ServiceResponse(string message, bool successful = false)
    {
        Message = message;
        Successful = successful;
    }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T Response { get; set; }

    public ServiceResponse(string message, bool successful = false, T response = default)
        : base(message, successful)
    {
        Response = response;
    }
}
