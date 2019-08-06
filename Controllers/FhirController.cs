using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using FhirXChangerService.Model;
using FhirXChangerService.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FhirXChangerService.Controllers
{
    [Route("api/[controller]")]
    public class FhirController : Controller
    {
        IFHIRXchangerService FHIRXchangerServiceObj;
        public FhirController(IFHIRXchangerService fhirXChangerService)
        {
            FHIRXchangerServiceObj = fhirXChangerService;
        }


        /// <summary>
        /// Converts XML file to FHIR json format and posts to FHIR server
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Post()
        {
            var doc = new XmlDocument();
            string text;
            var file = Request.Form.Files["cda"];
            using (Stream s = file.OpenReadStream())
            {

                StreamReader reader = new StreamReader(s);
                text = reader.ReadToEnd();
            }
            var fhirResonse = FHIRXchangerServiceObj.PostCda(text);
            return fhirResonse;
        }

        /// <summary>
        /// Fetch data from Fhir server
        /// </summary>
        /// <param name="id">ID for the FHIR resource</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(string id)
        {          
            var cdaBundle = FHIRXchangerServiceObj.GetCda(id);
            return JValue.Parse(cdaBundle).ToString();
        }

      

    }
}
