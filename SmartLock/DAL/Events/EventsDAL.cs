/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Events
{
    public class EventsDAL
    {
        IEventsData eventsDataLayer;

        public EventsDAL()
        {
            this.eventsDataLayer = new EventsData();
        }

        // unit testing purposes
        internal EventsDAL(IEventsData eventsData)
        {
            this.eventsDataLayer = eventsData;
        }

        public bool CreateEvent(int lockId, int userId, string lockState)
        {
            return this.eventsDataLayer.CreateEvent(lockId, userId, lockState);
        }

        public IList<string> GetUserEvents(int userId)
        {
            return this.eventsDataLayer.GetUserEvents(userId);
        }
    }
}