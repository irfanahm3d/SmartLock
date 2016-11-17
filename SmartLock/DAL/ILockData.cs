using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.DAL
{
    interface ILockData
    {
        string GetLockState(int lockId);

        bool ModifyLockState(int lockId, int userId, string state);
    }
}
