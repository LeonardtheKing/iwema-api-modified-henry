using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Dashboard.ActiveUsers;

public record GetActiveUserCountQuery() : IRequest<ServiceResponse<int>>;

public class GetActiveUserCountHandler : IRequestHandler<GetActiveUserCountQuery, ServiceResponse<int>>
{
    private readonly IUserSessionRepository _userLoginSessionRepository;

    public GetActiveUserCountHandler(IUserSessionRepository userLoginSessionRepository)
    {
        _userLoginSessionRepository = userLoginSessionRepository;
    }

    public async Task<ServiceResponse<int>> Handle(GetActiveUserCountQuery query, CancellationToken cancellationToken)
    {
        var loginTimes = await _userLoginSessionRepository.GetActiveUserLoginTimes();
        var activeUserCount = loginTimes.Count;
        return new ServiceResponse<int>(string.Empty, true, activeUserCount);
    }
}
