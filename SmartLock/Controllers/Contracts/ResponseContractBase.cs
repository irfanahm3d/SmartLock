/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Runtime.Serialization;

namespace SmartLock.Controllers.Contracts
{
    [DataContract]
    public class ResponseContractBase
    {
        [DataMember]
        public string Message { get; set; }
    }
}