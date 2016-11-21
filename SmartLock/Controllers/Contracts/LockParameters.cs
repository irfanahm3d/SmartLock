/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SmartLock.Controllers.Exceptions;

namespace SmartLock.Controllers.Contracts
{
    public enum LockState
    {
        None,
        Lock,
        Unlock
    }

    public class LockParameters
    {
        public int LockId { get; private set; }

        public int UserId { get; private set; }

        public string LockName { get; private set; }

        public LockState LockState { get; private set; }

        public IList<int> AllowedUsers { get; private set; }

        public static LockParameters ParseGetLockParameters(NameValueCollection queryParameters)
        {
            int lockId = 0;
            if (!Int32.TryParse(queryParameters["lockId"], out lockId))
            {
                throw new InvalidParameterException("lockId");
            }

            int userId = 0;
            if (!Int32.TryParse(queryParameters["userId"], out userId))
            {
                throw new InvalidParameterException("userId");
            }

            return new LockParameters
            {
                LockId = lockId,
                UserId = userId
            };
        }

        public static LockParameters ParseGetLocksParameters(NameValueCollection queryParameters)
        {
            int userId = 0;
            if (!Int32.TryParse(queryParameters["userId"], out userId))
            {
                throw new InvalidParameterException("userId");
            }

            return new LockParameters
            {
                UserId = userId
            };
        }

        public static LockParameters ParsePostLockParameters(NameValueCollection queryParameters)
        {
            int lockId = 0;
            if (!Int32.TryParse(queryParameters["lockId"], out lockId))
            {
                throw new InvalidParameterException("lockId");
            }

            int userId = 0;
            if (!Int32.TryParse(queryParameters["userId"], out userId))
            {
                throw new InvalidParameterException("userId");
            }

            string lockStateString = queryParameters["lockState"];
            LockState lockState;
            if (String.IsNullOrWhiteSpace(lockStateString) ||
                !Enum.TryParse<LockState>(lockStateString, true, out lockState))
            {
                throw new InvalidParameterException("lockState");
            }

            return new LockParameters
            {
                LockId = lockId,
                UserId = userId,
                LockState = lockState
            };
        }

        public static LockParameters ParsePutLockParameters(NameValueCollection queryParameters)
        {
            int userId = 0;
            if (!Int32.TryParse(queryParameters["userId"], out userId))
            {
                throw new InvalidParameterException("userId");
            }

            string lockName = queryParameters["lockName"];
            if (String.IsNullOrWhiteSpace(lockName))
            {
                throw new InvalidParameterException("lockName");
            }

            string allowedUsersString = queryParameters["allowedUsers"];

            if (String.IsNullOrWhiteSpace(allowedUsersString))
            {
                throw new InvalidParameterException("allowedUsers");
            }

            string[] allowedUsersList = allowedUsersString.Split(',');
            if (allowedUsersList.Length == 0)
            {
                throw new InvalidParameterException("allowedUsers");
            }

            var allowedUsers = new List<int>();
            foreach (string userIdString in allowedUsersList)
            {
                int validUserId = 0;
                if (!Int32.TryParse(userIdString, out validUserId))
                {
                    throw new InvalidParameterException("allowedUsers");
                }

                allowedUsers.Add(validUserId);
            }

            return new LockParameters
            {
                UserId = userId,
                LockName = lockName,
                AllowedUsers = allowedUsers
            };
        }
    }
}