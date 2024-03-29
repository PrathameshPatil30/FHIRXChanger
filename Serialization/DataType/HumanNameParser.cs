﻿using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Model;
using FhirXChangerService.Serialization.ValueSet;

namespace FhirXChangerService.Serialization.DataType
{
    public class HumanNameParser : BaseParser<HumanName>
    {
        public override HumanName FromXml(XElement element)
        {
            if (element == null)
                return null;

            var name = new HumanName
            {
                Use = new NameUseParser().FromCda(element.Attribute("use")?.Value)
            };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "family":
                        name.Family = child.Value;
                        break;
                    case "given":
                        var given = child.Value;
                        if (!string.IsNullOrEmpty(given))
                            name.GivenElement.Add(new FhirString(given));
                        break;
                    case "prefix":
                        var prefix = child.Value;
                        if (!string.IsNullOrEmpty(prefix))
                            name.PrefixElement.Add(new FhirString(prefix));
                        break;
                    case "suffix":
                        var suffix = child.Value;
                        if (!string.IsNullOrEmpty(suffix))
                            name.SuffixElement.Add(new FhirString(suffix));
                        break;
                }

            if (string.IsNullOrEmpty(name.Family))
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have family element", ParseErrorLevel.Error));
                return null;
            }

            if (!name.Given.Any())
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have given element", ParseErrorLevel.Error));
                return null;
            }

            return name;
        }
    }
}