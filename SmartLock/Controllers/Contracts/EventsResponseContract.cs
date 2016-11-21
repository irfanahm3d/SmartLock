/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SmartLock.DAL.Events;
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [DataContract]
    public class EventsResponseContract : ResponseContractBase
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<EventContract> EventsList { get; set; } 

        public IList<EventContract> ConvertToContract(IList<EventModel> eventModelList)
        {
            return eventModelList.Select(
                em => 
                new EventContract
                {
                    LockId = em.LockId,
                    State = em.State,
                    Timestamp = em.Timestamp
                }).ToList();            
        }
    }

    [DataContract]
    public class EventContract
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int LockId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public DateTime Timestamp { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string State { get; set; }

    }
}