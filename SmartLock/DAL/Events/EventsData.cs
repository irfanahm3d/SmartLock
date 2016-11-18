/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartLock.Models;

namespace SmartLock.DAL.Events
{
    public class EventsData : IEventsData
    {
        public bool CreateEvent(int lockId, int userId, string lockState)
        {
            using (var smartLock = new SmartLockEntities())
            {
                smartLock.UserEvents.Add(
                    new UserEvent
                    {
                        LockId = lockId,
                        UserId = userId,
                        State = lockState,
                        Timestamp = DateTime.Now
                    });

                int change = smartLock.SaveChanges();
                return change == 1 ? true : false;
            }
        }

        public IList<string> GetUserEvents(int userId)
        {
            using (var smartLock = new SmartLockEntities())
            {
                var query = from events in smartLock.UserEvents
                            where events.UserId == userId
                            select events;

                var eventsList = new List<string>();
                foreach(var element in query)
                {
                    eventsList.Add(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "{0};{1};{2}",
                            element.LockId,
                            element.Timestamp,
                            element.State));
                }
                return eventsList;
            }
        }
    }
}