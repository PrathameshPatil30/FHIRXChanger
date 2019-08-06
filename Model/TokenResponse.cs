using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FhirXChangerService.Model
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresInSeconds { get; set; }

        [JsonProperty("expires_on")]
        public int Expiration { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
