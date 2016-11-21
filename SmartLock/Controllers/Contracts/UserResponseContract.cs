/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */
 
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [JsonObject("user")]
    public class UserResponseContract : ResponseContractBase
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }
        
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        
        [JsonProperty("isAdmin", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAdmin { get; set; }
    }
}