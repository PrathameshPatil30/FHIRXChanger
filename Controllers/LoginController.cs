using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FhirXChangerService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FhirXChangerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var redirectUrl = FHIRXchangerReop.GenerateAzureURI();
            Response.Redirect(redirectUrl);
            return "Login page";
        }
    }
}