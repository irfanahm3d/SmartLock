/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [DataContract]
    public class LockResponseContract : ResponseContractBase
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int LockId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LockState { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LockName { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<int> AllowedUsers { get; set; }        
    }
}