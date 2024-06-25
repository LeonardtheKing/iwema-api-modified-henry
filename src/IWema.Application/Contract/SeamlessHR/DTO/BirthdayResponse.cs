using Newtonsoft.Json;

namespace IWema.Application.Contract.SeamlessHR.DTO;

public class BirthdayResponse
{
    public List<BirthdayResponseData> Data { get; set; }
    public MetaData Meta { get; set; }
}

public class BirthdayResponseData
{
    [JsonProperty("employee_code")]
    public string EmployeeCode { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("date_of_birth")]
    public string DateOfBirth { get; set; }
}
