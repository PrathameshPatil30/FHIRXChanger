using Hl7.Fhir.Model;

namespace FhirXChangerService.Serialization.ValueSet
{
    public class AllergyIntoleranceClinicalStatusParser
    {
        public AllergyIntolerance.AllergyIntoleranceClinicalStatus? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "active":
                    return AllergyIntolerance.AllergyIntoleranceClinicalStatus.Active;
                case "completed":
                    return AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved;
                default:
                    return AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive;
            }
        }
    }
}
