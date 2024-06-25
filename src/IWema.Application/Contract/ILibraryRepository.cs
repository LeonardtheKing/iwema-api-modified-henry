using IWema.Domain.Entity;

namespace IWema.Application.Contract
{
    public interface ILibraryRepository
    {
        Task<bool> Add(Library library);
        Task<bool> Delete(Guid id);
        Task<List<Library>> Get();
        Task<Library> GetById(Guid id);
    }
}
