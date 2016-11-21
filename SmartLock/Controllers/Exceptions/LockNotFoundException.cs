/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;

namespace SmartLock.Controllers.Exceptions
{
    public class LockNotFoundException : Exception
    {
        public LockNotFoundException()
        {
        }

        public LockNotFoundException(string message)
        : base(message)
        {
        }

        public LockNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}