using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FhirXChangerService.Controllers;
using FhirXChangerService.Model;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FhirXChangerService.Repositories
{
    public class FHIRXchangerReop : IFHIRXchangerRepo
    {
        private readonly IConfiguration _config;
        protected static string openFHIRUrl;
        protected static FhirClient fhirClient;
        protected static FhirJsonSerializer fhirJsonSerializer;
        protected const string Bundle = "Bundle";
        private string _result;

        public FHIRXchangerReop(IConfiguration config)
        {
            _config = config ??
                throw new ArgumentNullException($"{nameof(config)} cannot be null");
            openFHIRUrl = _config["FhirServerUrl"];

            fhirClient = new FhirClient(openFHIRUrl);
            fhirJsonSerializer = new FhirJsonSerializer();
        }

        public static string GenerateAzureURI()
        {
            string authenticationUrl =
                           string.Format(@"https://login.microsoftonline.com/fdfa7fae-9780-44a0-8721-26e3f588a7f0/oauth2/authorize?resource=https://azurehealthcareapis.com&client_id={0}" +
                               "&redirect_uri={1}" +
                               "&response_mode=form_post" +
                               "&response_type=code" +
                               "&scope={2}",

                           Uri.EscapeDataString("74fb1cfb-31e2-4820-9408-9a9c4995f9ac"),
                           HttpUtility.UrlEncode("http://localhost:23641/api/Auth"),
                           "openid+email+profile+offline_access");

            return authenticationUrl;
        }

        /// <summary>
        /// Fetch data from Fhir server
        /// </summary>
        /// <param name="id">ID for the FHIR resource</param>
        /// <returns></returns>
        public string Get(string id)
        {
            var response = GetAzureData(openFHIRUrl + "/" + Bundle + "/" + id);
            return response.Result;
        }

        private async Task<string> GetAzureData(string url)
        {            
            var azureEndpoint = url;
            try
            {
                var token = RequestAccessToken();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(azureEndpoint);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _result = await response.Content.ReadAsStringAsync();
                    }
                    else if (response.Content != null)
                    {
                        var _result = await response.Content.ReadAsStringAsync();
                    }

                    return _result;
                }
            }
            catch (FhirOperationException FhirOpExec)
            {
                throw FhirOpExec;
            }
            catch (WebException ex)
            {
                var res = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                var json = JsonConvert.DeserializeObject(res);
                return json.ToString();
            }
            catch (Exception GeneralException)
            {
                throw GeneralException;
            }
        }
   
        private static async Task<TokenResponse> RequestAccessToken()
        {
            TokenResponse tokenDetails = null;
            string endpointHost = "https://login.microsoftonline.com";
            string currentSiteCallbackUri = "http://localhost:23641/api/Auth";
            string grant_type = "authorization_code";
            string client_id = "74fb1cfb-31e2-4820-9408-9a9c4995f9ac";
            string client_secret = "DJL?I-F83zV@W=aRJGWf4Oxq20fOfUbn";
            string token_URL = "/fdfa7fae-9780-44a0-8721-26e3f588a7f0/oauth2/token";
            string contentType = "application/x-www-form-urlencoded";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpointHost);
                using (var request = new HttpRequestMessage(HttpMethod.Post, token_URL))
                {
                    StringContent contentParams = new StringContent(string.Format("grant_type={0}" +
                        "&redirect_uri={1}" +
                        "&client_id={2}" +
                        "&client_secret={3}" +
                        "&code={4}"
                        , grant_type,
                        Uri.EscapeDataString(currentSiteCallbackUri),
                        Uri.EscapeDataString(client_id),
                        client_secret,
                        Uri.EscapeDataString(AuthController.authCode)), Encoding.UTF8, contentType);
                    request.Content = contentParams;

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            tokenDetails = JsonConvert.DeserializeObject<TokenResponse>(json);
                        }
                        else if (response.Content != null)
                        {
                            var errorDetails = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }

            return tokenDetails;
        }

        //private static string GetData(string url)
        //{
        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Accept = "application/json";
        //    httpWebRequest.Method = "GET";

        //    string response;

        //    try
        //    {
        //        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            response = streamReader.ReadToEnd();
        //            return response;
        //        }
        //    }
        //    catch (FhirOperationException FhirOpExec)
        //    {
        //        throw FhirOpExec;
        //    }
        //    catch (WebException ex)
        //    {
        //        var res = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //        var json = JsonConvert.DeserializeObject(res);
        //        return json.ToString();
        //    }
        //    catch (Exception GeneralException)
        //    {
        //        throw GeneralException;
        //    }
        //}

        /// <summary>
        /// Converts XML file to FHIR json format and posts to FHIR server
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public string Post(Bundle bundle)
        {
            string jsonResponse;
            try
            {
                var token = RequestAccessToken(); 
                fhirClient.PreferredFormat = ResourceFormat.Json;
                fhirClient.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
                {
                    // Replace with a valid bearer token for this server
                    e.RawRequest.Headers.Add("Authorization", "Bearer " + token.Result.AccessToken);
                };
                var response = fhirClient.Create(bundle);
                jsonResponse = fhirJsonSerializer.SerializeToString(response);
                return jsonResponse;
            }
            catch (FhirOperationException FhirOpExec)
            {
                var response = FhirOpExec.Outcome;
                var errorDetails = fhirJsonSerializer.SerializeToString(response);
                jsonResponse = JValue.Parse(errorDetails).ToString();
            }
            catch (WebException ex)
            {
                var response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                var error = JsonConvert.DeserializeObject(response);
                jsonResponse = error.ToString();
            }
            return jsonResponse;
        }
    }
}
