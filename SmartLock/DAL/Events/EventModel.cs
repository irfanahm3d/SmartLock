/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;

namespace SmartLock.DAL.Events
{
    public class EventModel
    {
        public int LockId { get; set; }

        public DateTime Timestamp { get; set; }

        public string State { get; set; }
    }
}