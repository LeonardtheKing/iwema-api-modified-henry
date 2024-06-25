namespace IWema.Application.Libraries.Query.GetAll;

public record GetLibraryModel(Guid Id, string FullFileLocation, string Description, string Type, string DateCreated);
public record GetAllLibraryQueryOutputModel(IEnumerable<GetLibraryModel> Form, IEnumerable<GetLibraryModel> PolicyManual, IEnumerable<GetLibraryModel> Template, IEnumerable<GetLibraryModel> Report, IEnumerable<GetLibraryModel> Letter, IEnumerable<GetLibraryModel> ProductCompendium, IEnumerable<GetLibraryModel> Appendix);
