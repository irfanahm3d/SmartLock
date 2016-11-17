using SmartLock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartLock.DAL
{
    public class LockData : ILockData
    {
        public string GetLockState(int lockId)
        {
            using (SmartLockEntities smartLock = new SmartLockEntities())
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
            using (SmartLockEntities smartLock = new SmartLockEntities())
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
    }
}