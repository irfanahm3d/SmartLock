/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.Lock
{
    interface ILockData
    {
        IList<LockModel> GetLocks();

        string GetLockState(int lockId);

        bool ModifyLockState(int lockId, int userId, string state);

        LockModel CreateLock(string lockName, IList<int> allowedUsers);

        bool CreateUserAccess(int lockId, IList<int> allowedUsers);
    }
}
