/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SmartLock.DAL.Lock;

namespace SmartLock.Controllers.Contracts
{
    [JsonObject("locks")]
    public class LocksResponseContract : ResponseContractBase
    {
        [JsonProperty("userId", NullValueHandling=NullValueHandling.Include)]
        public int UserId { get; set; }

        [JsonProperty("locksList", NullValueHandling = NullValueHandling.Ignore)]
        public IList<LockContract> Locks { get; set; }

        public IList<LockContract> ConvertToContract(IList<LockModel> lockModelList)
        {
            return lockModelList.Select(
                lm =>
                    new LockContract
                    {
                        LockId = lm.LockId,
                        Name = lm.Name,
                        State = lm.State
                    }).ToList();
        }
    }

    [JsonObject("lock")]
    public class LockContract
    {
        [JsonProperty("lockId", NullValueHandling = NullValueHandling.Include)]
        public int LockId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Include)]
        public string State { get; set; }
    }
}