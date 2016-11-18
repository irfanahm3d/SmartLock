/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Lock
{
    public class LockDAL
    {
        ILockData lockDataLayer;

        public LockDAL()
        {
            this.lockDataLayer = new LockData();
        }

        // unit testing purposes
        internal LockDAL(ILockData lockData)
        {
            this.lockDataLayer = lockData;
        }

        public IList<string> GetLocksState()
        {
            return new List<string> { "value1", "value2" };
        }

        public string GetLockState(int lockId)
        {
            return this.lockDataLayer.GetLockState(lockId);
        }

        public bool ModifyLockState(int lockId, int userId, string state)
        {
            // check if user is allowed to change the state of the
            // lock (should happen in data layer).


            return this.lockDataLayer.ModifyLockState(lockId, userId, state);
        }

        public bool CreateLock(string lockName, IList<int> allowedUsers)
        {
            return this.lockDataLayer.CreateLock(lockName, allowedUsers);
        }
    }
}