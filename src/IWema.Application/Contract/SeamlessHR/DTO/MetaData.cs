namespace IWema.Application.Contract.SeamlessHR.DTO;

public class MetaData
{
    public int Total { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public int First_page { get; set; }
    public int Last_page { get; set; }
    public int Prev_page { get; set; }
    public int Next_page { get; set; }
    public int Current_page { get; set; }
    public int Per_page { get; set; } = 0;
}
