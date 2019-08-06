using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Extension;

namespace FhirXChangerService.Serialization.DataType
{
    public class CodeableConceptParser : BaseParser<CodeableConcept>
    {
        public override CodeableConcept FromXml(XElement element)
        {
            if (element == null)
                return null;

            var codeableConcept = new CodeableConcept();

            var coding = new CodingParser().FromXml(element, Errors);

            if (coding != null)
                codeableConcept.Coding.Add(coding);

            var transElements = element.CdaElements("translation");

            foreach (var transElement in transElements)
            {
                coding = new CodingParser().FromXml(transElement, Errors);

                if (coding == null)
                    continue;

                codeableConcept.Coding.Add(coding);
            }

            codeableConcept.Text = element.CdaElement("originalText")?.CdaElement("reference")?.Attribute("value")
                ?.Value.Trim();

            return codeableConcept;
        }
    }
}
