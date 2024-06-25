using IWema.Application.Contract.SeamlessHR.DTO;

namespace IWema.Application.Anniversary.Query.GetAnniversaries;
public record GetAnniversariesOutputModel(
    List<AnniversaryResponseData> Month,
    List<AnniversaryResponseData> Week,
    List<AnniversaryResponseData> Today);