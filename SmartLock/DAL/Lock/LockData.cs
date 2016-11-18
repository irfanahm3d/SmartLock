/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.Models;

namespace SmartLock.DAL.Lock
{
    public class LockData : ILockData
    {
        public string GetLockState(int lockId)
        {
            using (var smartLock = new SmartLockEntities())
            {
                LockInfo lockInfo = smartLock.LockInfoes.FirstOrDefault(l => l.LockId == lockId);

                if (lockInfo == null)
                {
                    throw new ArgumentException("lockId");
                }

                return lockInfo.State;
            }
        }

        public bool ModifyLockState(int lockId, int userId, string state)
        {
            using (var smartLock = new SmartLockEntities())
            {
                LockAccess lockAccess = 
                    smartLock.LockAccesses.FirstOrDefault(l => l.UserId == userId && l.LockId == lockId);

                // TODO: Expand check to limit access based on time
                if (lockAccess == null)
                {
                    // user does not have access to the lock. Throw exception.
                    // event should also be logged
                    smartLock.UserEvents.Add(
                    new UserEvent
                    {
                        LockId = lockId,
                        UserId = userId,
                        State = "Unauthorized",
                        Timestamp = DateTime.Now
                    });

                    smartLock.SaveChanges();

                    throw new ArgumentException("userId");
                }
                
                LockInfo lockInfo = smartLock.LockInfoes.FirstOrDefault(l => l.LockId == lockId);
                lockInfo.State = state;

                int changes = smartLock.SaveChanges();

                bool result = false;
                if (changes == 1)
                {
                    result = true;
                }
                else
                {
                    state = "Failed";
                }

                smartLock.UserEvents.Add(
                    new UserEvent
                    {
                        LockId = lockId,
                        UserId = userId,
                        State = state,
                        Timestamp = DateTime.Now
                    });
                smartLock.SaveChanges();
                return result;
            }
        }

        public bool CreateLock(string lockName, IList<int> allowedUsers)
        {
            using (var smartLock = new SmartLockEntities())
            {
                int changes = 0;
                var lockInfo = new LockInfo
                {
                    Name = lockName
                };

                smartLock.LockInfoes.Add(lockInfo);
                changes += smartLock.SaveChanges();

                if (allowedUsers.Count > 0)
                {
                    smartLock.LockAccesses.AddRange(this.PopulateLockAccessList(lockInfo.LockId, allowedUsers));
                    changes += smartLock.SaveChanges();
                }

                return (changes == allowedUsers.Count + 1) ? true : false;
            }
        }

        public bool CreateUserAccess(int lockId, IList<int> allowedUsers)
        {
            using (var smartLock = new SmartLockEntities())
            {
                smartLock.LockAccesses.AddRange(this.PopulateLockAccessList(lockId, allowedUsers));
                int changes = smartLock.SaveChanges();

                return changes == allowedUsers.Count ? true : false;
            }
        }

        IList<LockAccess> PopulateLockAccessList(int lockId, IList<int> allowedUsers)
        {
            var lockAccessList = new List<LockAccess>();
            foreach (int userId in allowedUsers)
            {
                lockAccessList.Add(
                    new LockAccess
                    {
                        LockId = lockId,
                        UserId = userId
                    });
            }

            return lockAccessList;
        }
    }
}