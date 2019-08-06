using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Extension;
using FhirXChangerService.Serialization.DataType;

namespace FhirXChangerService.Serialization.Resource
{
    public class DeviceParser : BaseParser<Device>
    {
        public DeviceParser()
        {
        }

        public DeviceParser(Bundle bundle) : base(bundle)
        {
        }

        public override Device FromXml(XElement element)
        {
            if (element == null)
                return null;

            var device = new Device
            {
                Id = Guid.NewGuid().ToString()
            };

            var location = new Location
            {
                Id = Guid.NewGuid().ToString()
            };

            Bundle?.AddResourceEntry(device);

            foreach (var child in element.Elements())
                if (child.Name.LocalName == "id")
                {
                    var id = FromXml(new IdentifierParser(), child);

                    if (id != null)
                        device.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "code")
                {
                    device.Type = FromXml(new CodeableConceptParser(), child);
                }
                else if (child.Name.LocalName == "addr")
                {
                    location.Address = FromXml(new AddressParser(), child);
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var telecom = FromXml(new ContactPointParser(), child);
                    if (telecom != null)
                        location.Telecom.Add(telecom);
                }
                else if (child.Name.LocalName == "assignedAuthoringDevice")
                {
                    device.Model = child.CdaElement("manufacturerModelName")?.Value;
                }


            return device;
        }
    }
}