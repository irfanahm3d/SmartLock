/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartLock.Models;

namespace SmartLock.DAL.Lock
{
    public class LockData : ILockData
    {
        public IList<string> GetLocks()
        {
            using (var smartLock = new SmartLockEntities())
            {
                var query = from locks in smartLock.LockInfoes
                            select locks;

                var locksList = new List<string>();
                foreach(LockInfo element in query)
                {
                    locksList.Add(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "{0};{1}",
                            element.Name,
                            element.State));
                }

                return locksList;
            }
        }

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
                    throw new ArgumentException("userId");
                }
                
                LockInfo lockInfo = smartLock.LockInfoes.FirstOrDefault(l => l.LockId == lockId);
                lockInfo.State = state;

                int changes = smartLock.SaveChanges();                
                return changes == 1 ? true : false;
            }
        }

        public bool CreateLock(string lockName, int currentUserId, IList<int> allowedUsers)
        {
            using (var smartLock = new SmartLockEntities())
            {
                int changes = 0;

                UserInfo adminUser = smartLock.UserInfoes.FirstOrDefault(u => u.UserId == currentUserId && u.IsAdmin);

                if (adminUser == null)
                {
                    // user not allowed to create locks. Is not admin
                    throw new ArgumentException("currentUserId");
                }

                var lockInfo = new LockInfo
                {
                    Name = lockName,
                    State = "Locked"
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