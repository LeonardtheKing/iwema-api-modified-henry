using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWema.Application.Libraries.Query.GetById
{
    public record GetLibraryByIdQueryOutputModel(Guid Id, string Description, string Type, string DateCreated);
   
}
