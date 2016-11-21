/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [JsonObject("lock")]
    public class LockResponseContract : ResponseContractBase
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Include)]
        public int LockId { get; set; }

        [JsonProperty("userId", NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string LockState { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string LockName { get; set; }

        [JsonProperty("allowedUsers", NullValueHandling = NullValueHandling.Ignore)]
        public IList<int> AllowedUsers { get; set; }
    }
}