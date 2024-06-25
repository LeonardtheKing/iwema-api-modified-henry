using IWema.Application.Common.DTO;
using IWema.Application.Contract.SeamlessHR.DTO;

namespace IWema.Application.Contract.SeamlessHR;

public interface IBirthDayService
{
    Task<ServiceResponse<BirthdayResponse>> GetBirthdayCelebrants();
}
