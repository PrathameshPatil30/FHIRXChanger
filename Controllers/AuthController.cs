using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FhirXChangerService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FhirXChangerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static string authCode = null;

        [HttpPost]
        public string Get()
        {
            authCode = HttpContext.Request.Form["code"].ToString();

            return authCode;
        }
    }
}