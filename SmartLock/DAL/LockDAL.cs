using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartLock.DAL
{
    public class LockDAL
    {
        ILockData lockDataLayer;

        public LockDAL()
        {
            lockDataLayer = new LockData();
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
            return lockDataLayer.GetLockState(lockId);
        }

        public bool ModifyLockState(int lockId, int userId, string state)
        {
            // check if user is allowed to change the state of the
            // lock (should happen in data layer).


            return lockDataLayer.ModifyLockState(lockId, userId, state);
        }
    }
}