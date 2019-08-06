using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FhirXChangerService.Model
{
    public class FhirViewerModel
    {
        public string Id { get; set; }

        public Patient Patient { get; set; }

        public AllergyIntolerance AllergyInfo { get; set; }
        
        public RelatedPerson RelatedPerson { get; set; }

        public Organization Organization { get; set; }
    }
}
