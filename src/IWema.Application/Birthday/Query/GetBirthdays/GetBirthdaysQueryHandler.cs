using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.SeamlessHR;
using MediatR;

namespace IWema.Application.Birthday.Query.GetBirthdays;

public record GetBirthdaysQuery : IRequest<ServiceResponse<GetBirthdaysOutputModel>>;

internal class GetBirthdaysQueryHandler(IBirthDayService birthDayService, ICachingAdapter cachingAdapter) : IRequestHandler<GetBirthdaysQuery, ServiceResponse<GetBirthdaysOutputModel>>
{
    public async Task<ServiceResponse<GetBirthdaysOutputModel>> Handle(GetBirthdaysQuery request, CancellationToken cancellationToken = default)
    {
        var cachedBirthdays = cachingAdapter.GetCache<GetBirthdaysOutputModel>(nameof(GetBirthdaysOutputModel));
        if (cachedBirthdays != null)
        {
            return new("", true, cachedBirthdays);
        }

        var birthdayServiceResponse = await birthDayService.GetBirthdayCelebrants();    
        if (!birthdayServiceResponse.Successful)
        {
            return new(birthdayServiceResponse.Message);
        }

        var today = DateTime.UtcNow.AddHours(1);

        var month = birthdayServiceResponse.Response.Data
            .Where(b => DateTime.Parse(b.DateOfBirth).Month == today.Month)
            .ToList();

        var firstDayOfTheWeek = today.AddDays(-(int)today.DayOfWeek);
        var lastDayOfTheWeek = firstDayOfTheWeek.AddDays(6);

        var week = month.Where(b => DateTime.Parse(b.DateOfBirth).Month == today.Month &&
            DateTime.Parse(b.DateOfBirth).Day >= firstDayOfTheWeek.Day &&
            DateTime.Parse(b.DateOfBirth).Day <= lastDayOfTheWeek.Day)
            .ToList();

        var day = week.Where(b => DateTime.Parse(b.DateOfBirth).Day == today.Day).ToList();

        var birthdayUuoputModel = new GetBirthdaysOutputModel(month, week, day);

        cachingAdapter.AddCache(nameof(GetBirthdaysOutputModel), birthdayUuoputModel);

        return new("", true, birthdayUuoputModel);
    }
}
