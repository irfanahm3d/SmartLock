/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLock.DAL.Events;
using SmartLock.Models;

namespace SmartLock.Tests.DAL
{
    public class MockEventsData : IEventsData
    {
        IList<UserEvent> EventData = new List<UserEvent>
        {
            new UserEvent
            {
                Id = 1,
                LockId = 30,
                UserId = 20,
                State = "Lock",
                Timestamp = DateTime.UtcNow.AddMinutes(-10)
            },
        };

        public bool CreateEvent(int lockId, int userId, string lockState)
        {
            this.EventData.Add(
                new UserEvent
                {
                    Id = this.EventData.Count + 1,
                    LockId = lockId,
                    UserId = userId,
                    State = lockState,
                    Timestamp = DateTime.UtcNow
                });

            return true;
        }

        public IList<EventModel> GetUserEvents(int userId)
        {
            return 
                this.EventData.Where(e => e.UserId == userId).Select(
                    e =>
                        new EventModel
                        {
                            LockId = e.LockId,
                            State = e.State,
                            Timestamp = e.Timestamp
                        }).ToList();
        }
    }
}
