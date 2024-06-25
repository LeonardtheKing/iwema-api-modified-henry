namespace IWema.Infrastructure.Adapters;

public class RateLimiting
{
    public int WindowTimeSpan { get; set; }
    public int PermitLimit { get; set; }
    public int QueueLimit { get; set; }
    public int RejectionStatusCode { get; set; }


}
