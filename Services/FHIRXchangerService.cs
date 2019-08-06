using FhirXChangerService.Repositories;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FhirXChangerService.Services
{
    public class FHIRXchangerService : IFHIRXchangerService
    {
        IFHIRXchangerRepo FHIRXchangeRepoObj;
        public FHIRXchangerService(IFHIRXchangerRepo fhirxchangerRepo)
        {
            FHIRXchangeRepoObj = fhirxchangerRepo;
        }

        public string PostCda(string doc)
        {
            var xml = XDocument.Parse(doc);

            var parserSettings = new CdaParserSettings
            {
                RunValidation = false
            };

            var parser = new CdaParser(parserSettings);
            var bundle = parser.Convert(xml);
            var docPosted = FHIRXchangeRepoObj.Post(bundle);

            return docPosted;

        }

        public string GetCda(string docId)
        {
            return FHIRXchangeRepoObj.Get(docId);
        }
    }
}
