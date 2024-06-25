using IWema.Application.Contract.SeamlessHR.DTO;

namespace IWema.Application.Birthday.Query.GetBirthdays;
public record GetBirthdaysOutputModel(
    List<BirthdayResponseData> Month, 
    List<BirthdayResponseData> Week, 
    List<BirthdayResponseData> Today);