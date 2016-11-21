/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using SmartLock.Controllers.Contracts;
using SmartLock.DAL.Lock;
using SmartLock.Models;
using SmartLock.Controllers.Exceptions;
using System.Linq;

namespace SmartLock.Tests.DAL
{
    public class MockLockData : ILockData
    {
        IDictionary<int, LockInfo> LockData = new Dictionary<int, LockInfo>
        {
            {
                30,
                new LockInfo
                {
                    LockId = 30,
                    Name = "Office entrance",
                    State = "Locked"
                }
            },
            {
                31,
                new LockInfo
                {
                    LockId = 31,
                    Name = "Tunnel",
                    State = "Locked"
                }
            },
            {
                32,
                new LockInfo
                {
                    LockId = 32,
                    Name = "Main door",
                    State = "Locked"
                }
            }
        };

        IList<LockAccess> LockAccessData = new List<LockAccess>
        {
            new LockAccess
            {
                Id = 1,
                UserId = 20,
                LockId = 30
            },
            new LockAccess
            {
                Id = 2,
                UserId = 20,
                LockId = 31
            },
            new LockAccess
            {
                Id = 3,
                UserId = 22,
                LockId = 30
            },

            new LockAccess
            {
                Id = 4,
                UserId = 22,
                LockId = 31
            },

            new LockAccess
            {
                Id = 5,
                UserId = 22,
                LockId = 32
            },

            new LockAccess
            {
                Id = 6,
                UserId = 21,
                LockId = 32
            },
        };

        int lockId;
        int accessId;

        public MockLockData()
        {
            this.lockId = 33;
            this.accessId = 7;
        }

        public LockModel CreateLock(string lockName, IList<int> allowedUsers)
        {
            this.LockData.Add(
                this.lockId,
                new LockInfo
                {
                    LockId = this.lockId,
                    Name = lockName,
                    State = "Locked" });

            foreach (int user in allowedUsers)
            {
                this.LockAccessData.Add(
                    new LockAccess
                    {
                        Id = this.accessId,
                        LockId = this.lockId,
                        UserId = user
                    });
                this.accessId++;
            }

            var lockModel = new LockModel
            {
                LockId = this.lockId,
                Name = lockName,
                State = "Locked",
                AllowedUsers = allowedUsers
            };

            this.lockId++;
            return lockModel;
        }

        public bool CreateUserAccess(int lockId, IList<int> allowedUsers)
        {
            throw new NotImplementedException();
        }

        public IList<LockModel> GetLocks()
        {
            throw new NotImplementedException();
        }

        public string GetLockState(int lockId)
        {
            LockInfo lockInfo;
            if (!this.LockData.TryGetValue(lockId, out lockInfo))
            {
                throw new LockNotFoundException("Not found.");
            }

            return lockInfo.State;
        }

        public bool ModifyLockState(int lockId, int userId, LockState state)
        {
            LockInfo lockInfo;
            if (!this.LockData.TryGetValue(lockId, out lockInfo))
            {
                throw new LockNotFoundException("Not found.");
            }

            LockAccess lockAccess = 
                this.LockAccessData.SingleOrDefault(l => l.UserId == userId && l.LockId == lockId);

            if (lockAccess == null)
            {
                throw new UnauthorizedUserException("Unauthorized");
            }

            lockInfo.State = String.Concat(state.ToString(), "ed");

            return true;
        }
    }
}
