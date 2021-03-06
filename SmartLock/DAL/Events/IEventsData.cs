﻿/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Events
{
    interface IEventsData
    {
        bool CreateEvent(int lockId, int userId, string lockState);

        IList<EventModel> GetUserEvents(int userId);
    }
}
