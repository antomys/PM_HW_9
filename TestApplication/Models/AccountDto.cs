using System;
using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class AccountDto
    {
        [JsonProperty("SessionId")]
        public string SessionId { get; set; }
        [JsonProperty("Login")]
        public string Login { get; set; }
        
        [JsonProperty("Password")]
        public string Password { get; set; }
        
        [JsonProperty("LastRequest")]
        
        public DateTime LastRequest { get; set; }
    }
}