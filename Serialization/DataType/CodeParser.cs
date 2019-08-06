using Hl7.Fhir.Model;
using System.Xml.Linq;

namespace FhirXChangerService.Serialization.DataType
{
    public class CodeParser : BaseParser<Code>
    {
        public override Code FromXml(XElement element)
        {
            var value = element?.Attribute("code")?.Value;

            return string.IsNullOrWhiteSpace(value) ? null : new Code(value);
        }
    }
}