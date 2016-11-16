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
            throw new NotImplementedException();
        }

        public string ModifyLockState(int lockId, string state)
        {
            throw new NotImplementedException();
        }
    }
}