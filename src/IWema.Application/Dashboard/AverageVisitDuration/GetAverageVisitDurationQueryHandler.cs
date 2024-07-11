//using IWema.Application.Common.DTO;
//using IWema.Application.Contract;
//using IWema.Domain.Entity;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//public record AverageVisitDurationQuery : IRequest<ServiceResponse<double>>
//{
//    public DateTime? Date { get; set; }
//    public DateTime? StartDate { get; set; }
//    public DateTime? EndDate { get; set; }
//}


//public class AverageVisitDurationQueryHandler(IUserSessionRepository userSessionRepository) : IRequestHandler<AverageVisitDurationQuery, ServiceResponse<double>>
//{
//    public async Task<ServiceResponse<double>> Handle(AverageVisitDurationQuery request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            IQueryable<UserLoginSession> sessionsQuery = await userSessionRepository.GetQueryableSession();

//            if (request.Date.HasValue)
//            {
//                DateTime startDate = request.Date.Value.Date;
//                DateTime endDate = startDate.AddDays(1).AddTicks(-1);
//                sessionsQuery = sessionsQuery.Where(s => s.LoginTime >= startDate && s.LoginTime <= endDate);
//            }
//            else if (request.StartDate.HasValue && request.EndDate.HasValue)
//            {
//                DateTime startDate = request.StartDate.Value.Date;
//                DateTime endDate = request.EndDate.Value.Date.AddDays(1).AddTicks(-1);
//                sessionsQuery = sessionsQuery.Where(s => s.LoginTime >= startDate && s.LoginTime <= endDate);
//            }

//            var sessions = await sessionsQuery.ToListAsync();
//            var totalCount = sessions.Count;

//            if (totalCount > 0)
//            {
//                var totalDurationInSeconds = sessions.Sum(s => s.DurationInSeconds?.TotalSeconds ?? 0);
//                var averageDurationInHours = totalDurationInSeconds / 3600.0; // Convert total seconds to hours
//                return new ServiceResponse<double>("Average visit duration calculated successfully.", true, averageDurationInHours);
//            }
//            else
//            {
//                return new ServiceResponse<double>("No sessions found matching the criteria.", false, 0);
//            }
//        }
//        catch (Exception ex)
//        {
//            return new ServiceResponse<double>($"Error calculating average visit duration: {ex.Message}", false, 0);
//        }
//    }
//}


using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public record AverageVisitDurationQuery : IRequest<ServiceResponse<string>>
{
    public DateTime? Date { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetAverageVisitDurationQueryHandler : IRequestHandler<AverageVisitDurationQuery, ServiceResponse<string>>
{
    private readonly IUserSessionRepository _userSessionRepository;

    public GetAverageVisitDurationQueryHandler(IUserSessionRepository userSessionRepository)
    {
        _userSessionRepository = userSessionRepository;
    }

    public async Task<ServiceResponse<string>> Handle(AverageVisitDurationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<UserLoginSession> sessionsQuery = await _userSessionRepository.GetQueryableSession();

            if (request.Date.HasValue)
            {
                DateTime startDate = request.Date.Value.Date;
                DateTime endDate = startDate.AddDays(1).AddTicks(-1);
                sessionsQuery = sessionsQuery.Where(s => s.LoginTime >= startDate && s.LoginTime <= endDate);
            }
            else if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                DateTime startDate = request.StartDate.Value.Date;
                DateTime endDate = request.EndDate.Value.Date.AddDays(1).AddTicks(-1);
                sessionsQuery = sessionsQuery.Where(s => s.LoginTime >= startDate && s.LoginTime <= endDate);
            }
            else
            {
                return new ServiceResponse<string>("Invalid date range specified.", false, null);
            }

            var sessions = await sessionsQuery.ToListAsync(cancellationToken);
            var totalCount = sessions.Count;

            if (totalCount > 0)
            {
                var totalDurationInSeconds = sessions.Sum(s => s.DurationInSeconds?.TotalSeconds ?? 0);
                var averageDurationInSeconds = totalDurationInSeconds;

                var timeSpan = TimeSpan.FromSeconds(averageDurationInSeconds);
                var formattedDuration = string.Format("{0} hour{1} {2} minute{3} {4} second{5}",
                    (int)timeSpan.TotalHours, timeSpan.TotalHours != 1 ? "s" : "",
                    timeSpan.Minutes, timeSpan.Minutes != 1 ? "s" : "",
                    timeSpan.Seconds, timeSpan.Seconds != 1 ? "s" : "");

                return new ServiceResponse<string>("Average visit duration calculated successfully.", true, formattedDuration);
            }
            else
            {
                return new ServiceResponse<string>("No sessions found matching the criteria.", false, null);
            }
        }
        catch (Exception ex)
        {
            return new ServiceResponse<string>($"Error calculating average visit duration: {ex.Message}", false, null);
        }
    }
}

