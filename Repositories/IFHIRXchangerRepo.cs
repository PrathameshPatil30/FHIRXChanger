using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FhirXChangerService.Repositories
{
    public interface IFHIRXchangerRepo
    {
        string Post(Bundle bundle);
        string Get(string id);
    }
}
