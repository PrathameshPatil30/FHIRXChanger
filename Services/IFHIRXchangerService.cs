using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FhirXChangerService.Services
{
    public interface IFHIRXchangerService
    {
        string PostCda(string doc);
        string GetCda(string docId);
    }
}
