using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace FhirXChangerService.Serialization.DataType
{
    public class FhirBooleanParser : BaseParser<FhirBoolean>
    {
        public override FhirBoolean FromXml(XElement element)
        {
            var value = element?.Attribute("value")?.Value;

            return bool.TryParse(value, out bool result) ? new FhirBoolean(result) : null;
        }
    }
}
