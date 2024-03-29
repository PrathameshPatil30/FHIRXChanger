﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Extension;
using FhirXChangerService.Model;
using FhirXChangerService.Serialization.DataType;

namespace FhirXChangerService.Serialization.Resource
{
    public class OrganizationParser : BaseParser<Organization>
    {
        public OrganizationParser()
        {
        }

        public OrganizationParser(Bundle bundle) : base(bundle)
        {
        }

        public override Organization FromXml(XElement element)
        {
            if (element == null)
                return null;

            var org = new Organization
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta
                {
                    Profile = new List<string>
                    {
                        "http://hl7.org/fhir/us/core/StructureDefinition/us-core-organization"
                    }
                },
                Active = true
            };

            foreach (var child in element.Elements())
                if (child.Name.LocalName == "id")
                {
                    var id = new IdentifierParser().FromXml(child, Errors);

                    if (id != null)
                        org.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "name")
                {
                    org.Name = child.Value;
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var telecom = new ContactPointParser().FromXml(child, Errors);

                    if (telecom != null)
                        org.Telecom.Add(telecom);
                }
                else if (child.Name.LocalName == "addr")
                {
                    var addr = new AddressParser().FromXml(child, Errors);

                    if (addr != null)
                        org.Address.Add(addr);
                }

            if (!org.Identifier.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have id element", ParseErrorLevel.Error));

            if (string.IsNullOrEmpty(org.Name))
                Errors.Add(ParserError.CreateParseError(element, "does NOT have name element", ParseErrorLevel.Error));

            if (!org.Telecom.Any())
                Errors.Add(
                    ParserError.CreateParseError(element, "does NOT have telecom element", ParseErrorLevel.Error));

            if (!org.Address.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have addr element", ParseErrorLevel.Error));

            var existingOrg = Bundle?.FirstOrDefault<Organization>(p => p.Identifier.IsExactly(org.Identifier));

            if (existingOrg == null)
                Bundle?.AddResourceEntry(org);
            else
                org = existingOrg;

            return org;
        }
    }
}