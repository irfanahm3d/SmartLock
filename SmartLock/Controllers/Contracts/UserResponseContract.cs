/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [DataContract]
    public class UserResponseContract : ResponseContractBase
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAdmin { get; set; }
    }
}