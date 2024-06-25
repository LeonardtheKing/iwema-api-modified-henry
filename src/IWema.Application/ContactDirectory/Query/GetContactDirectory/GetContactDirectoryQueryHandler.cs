using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.SeamlessHR;
using IWema.Application.Contract.SeamlessHR.DTO;
using MediatR;

namespace IWema.Application.ContactDirectory.Query.GetContactDirectory;

public record GetContactDirectoryQuery(string? SearchTerm) : IRequest<ServiceResponse<List<ContactDirectoryResponseData>>>;

internal class GetContactDirectoryQueryHandler(IContactDirectoryService contactDirectoryService) : IRequestHandler<GetContactDirectoryQuery, ServiceResponse<List<ContactDirectoryResponseData>>>
{
    public async Task<ServiceResponse<List<ContactDirectoryResponseData>>> Handle(GetContactDirectoryQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var contactDirectoryResponse = await contactDirectoryService.GetContactDirectories(request.SearchTerm);
            if (!contactDirectoryResponse.Successful)
            {
                return new(contactDirectoryResponse.Message);
            }

            var activeStaff = contactDirectoryResponse.Response;
            List<ContactDirectoryResponseData> filteredStaff;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                filteredStaff = activeStaff.Where(staff =>
                    staff.StaffName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    staff.StaffEmail.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    staff.StaffPhone.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                filteredStaff = activeStaff;
            }

            return new("", true, filteredStaff);
        }
        catch (Exception ex)
        {
            return new("An error occurred while fetching Contact Directory");
        }
    }
}
