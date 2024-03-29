﻿using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Model;

namespace FhirXChangerService.Serialization.DataType
{
    public class DateParser : BaseParser<Date>
    {
        public override Date FromXml(XElement element)
        {
            if (element == null)
                return null;

            var dateString = element.Attribute("value")?.Value;

            if (dateString == null)
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have value attribute",
                    ParseErrorLevel.Error));
                return null;
            }

            if (dateString.Length < 4 || dateString.Length > 8)
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have valid value attribute",
                    ParseErrorLevel.Error));
                return null;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < dateString.Length; i++)
            {
                if (i == 4 || i == 6)
                    sb.Append("-");

                sb.Append(dateString[i]);
            }

            return new Date(sb.ToString());
        }
    }
}
