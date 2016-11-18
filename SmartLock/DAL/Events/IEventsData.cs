/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Events
{
    public interface IEventsData
    {
        bool CreateEvent(int lockId, int userId, string lockState);

        IList<string> GetUserEvents(int userId);
    }
}
