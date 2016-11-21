/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Lock
{
    public class LockModel
    {
        public int LockId { get; set; }

        public string Name { get; set; }

        public string State { get; set; }

        public IList<int> AllowedUsers { get; set; }
    }
}