using IWema.Application.Contract.SeamlessHR.DTO;

namespace IWema.Application.ContactDirectory.Query.GetContactDirectory;

public class GetContactDirectoryOutputModel
{
    public List<ContactDirectoryResponseData> Data { get; set; } = new List<ContactDirectoryResponseData>();
}


