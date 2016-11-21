/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    public class ResponseContractBase
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Include)]
        public string Message { get; set; }
    }
}