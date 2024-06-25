using Newtonsoft.Json;

namespace IWema.Application.Contract.SeamlessHR.DTO;

public class AnniversaryResponse
{
    public List<AnniversaryResponseData> Data { get; set; }
    public MetaData Meta { get; set; }
}

public class AnniversaryResponseData
{
    [JsonProperty("employee_code")]
    public string EmployeeCode { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("employment_date")]
    public string EmploymentDate { get; set; }

    public string Years { get; set; }
    public string Summary { get; set; }
}
