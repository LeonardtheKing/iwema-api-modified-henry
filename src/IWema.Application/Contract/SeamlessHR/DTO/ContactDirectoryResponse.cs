using Newtonsoft.Json;

namespace IWema.Application.Contract.SeamlessHR.DTO;

public class ContactDirectoryResponse
{
    public List<ContactDirectoryResponseData> Data { get; set; }
}

public class ContactDirectoryResponseData
{

    [JsonProperty("staff_name")]
    public string StaffName { get; set; } = string.Empty;

    [JsonProperty("staff_email")]
    public string StaffEmail { get; set; } = string.Empty;

    [JsonProperty("staff_phone")]
    public string StaffPhone { get; set; } = string.Empty;

    [JsonProperty("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonProperty("department")]
    public string Department { get; set; } = string.Empty;

    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [JsonProperty("location")]
    public string Location { get; set; } = string.Empty;

}