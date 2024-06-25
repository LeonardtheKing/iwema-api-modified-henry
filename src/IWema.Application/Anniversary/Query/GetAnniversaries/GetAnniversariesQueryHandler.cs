using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.SeamlessHR;
using MediatR;

namespace IWema.Application.Anniversary.Query.GetAnniversaries;

public record GetAnniversariesQuery : IRequest<ServiceResponse<GetAnniversariesOutputModel>>;
internal class GetAnniversariesQueryHandler(IAnniversaryService anniversaryDayService, ICachingAdapter cachingAdapter) : IRequestHandler<GetAnniversariesQuery, ServiceResponse<GetAnniversariesOutputModel>>
{
    public async Task<ServiceResponse<GetAnniversariesOutputModel>> Handle(GetAnniversariesQuery request, CancellationToken cancellationToken=default)
    {
        var cachedAnniversaries = cachingAdapter.GetCache<GetAnniversariesOutputModel>(nameof(GetAnniversariesOutputModel));
        if (cachedAnniversaries != null)
        {
            return new("", true, cachedAnniversaries);
        }

        var anniversariesResponse = await anniversaryDayService.GetAnniversaries();
        if (!anniversariesResponse.Successful)
        {
            return new(anniversariesResponse.Message);
        }

        var today = DateTime.UtcNow.AddHours(1);

        var month = anniversariesResponse.Response.Data
            .Where(b => DateTime.Parse(b.EmploymentDate).Month == today.Month)
            .ToList();

        var firstDayOfTheWeek = today.AddDays(-(int)today.DayOfWeek);
        var lastDayOfTheWeek = firstDayOfTheWeek.AddDays(6);

        var week = month.Where(b => DateTime.Parse(b.EmploymentDate).Month == today.Month &&
            DateTime.Parse(b.EmploymentDate).Day >= firstDayOfTheWeek.Day &&
            DateTime.Parse(b.EmploymentDate).Day <= lastDayOfTheWeek.Day)
            .ToList();

        var day = week.Where(b => DateTime.Parse(b.EmploymentDate).Day == today.Day).ToList();
        var anniversariesOputModel = new GetAnniversariesOutputModel(month, week, day);

        cachingAdapter.AddCache(nameof(GetAnniversariesOutputModel), anniversariesOputModel);

        return new("", true, anniversariesOputModel);
    }
}

