using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FhirXChangerService.Model;
using FhirXChangerService.Services;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FhirXChangerService.Controllers
{
    public class FhirViewerController : Controller
    {
        private static FhirJsonParser fhirJsonParser = new FhirJsonParser();

        private string cdaBundle;
        protected IFHIRXchangerService _iFHIRXchangerService { get; set; }

        public FhirViewerController(IFHIRXchangerService fHIRXchangerService)
        {
            _iFHIRXchangerService = fHIRXchangerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FhirDataViewer(FhirViewerModel vm)
        {

            FhirViewerModel fhirViewerModel = new FhirViewerModel();

            var fhirData = _iFHIRXchangerService.GetCda(vm.Id);

            Bundle Bundle = (Bundle)fhirJsonParser.Parse(fhirData, typeof(Bundle));
            var BundleList = Bundle.Entry.Select(Resource => Resource.Resource).ToList();

            foreach (var items in BundleList)
            {
                if (items.TypeName.Equals("Patient"))
                {
                    var patientJson = JsonConvert.SerializeObject(items.ToJson());
                    var patientJsonString = JValue.Parse(patientJson).ToString();
                    fhirViewerModel.Patient = (Patient)fhirJsonParser.Parse(patientJsonString, typeof(Patient));
                }
                else if (items.TypeName.Equals("AllergyIntolerance"))
                {
                    var allergyIntoleranceJson = JsonConvert.SerializeObject(items.ToJson());
                    var allergyIntoleranceJsonString = JValue.Parse(allergyIntoleranceJson).ToString();
                    fhirViewerModel.AllergyInfo = (AllergyIntolerance)fhirJsonParser.Parse(allergyIntoleranceJsonString, typeof(AllergyIntolerance));
                }
                else if (items.TypeName.Equals("RelatedPerson"))
                {
                    var relatedPersonJson = JsonConvert.SerializeObject(items.ToJson());
                    var relatedPersonJsonString = JValue.Parse(relatedPersonJson).ToString();
                    fhirViewerModel.RelatedPerson = (RelatedPerson)fhirJsonParser.Parse(relatedPersonJsonString, typeof(RelatedPerson));
                }
                else if (items.TypeName.Equals("Organization"))
                {
                    var organizationJson = JsonConvert.SerializeObject(items.ToJson());
                    var organizationJsonString = JValue.Parse(organizationJson).ToString();
                    fhirViewerModel.Organization = (Organization)fhirJsonParser.Parse(organizationJsonString, typeof(Organization));
                }
            }
            return View(fhirViewerModel);
        }
    }
}