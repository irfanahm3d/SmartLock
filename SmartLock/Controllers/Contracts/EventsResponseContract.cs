/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.DAL.Events;
using Newtonsoft.Json;

namespace SmartLock.Controllers.Contracts
{
    [JsonObject("events")]
    public class EventsResponseContract : ResponseContractBase
    {
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Include)]
        public int UserId { get; set; }
        
        [JsonProperty("eventList", NullValueHandling = NullValueHandling.Ignore)]
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

    [JsonObject("event")]
    public class EventContract
    {
        [JsonProperty("lockId", NullValueHandling = NullValueHandling.Include)]
        public int LockId { get; set; }
        
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Include)]
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("state", NullValueHandling = NullValueHandling.Include)]
        public string State { get; set; }

    }
}