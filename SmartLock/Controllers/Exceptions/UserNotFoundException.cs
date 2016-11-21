/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;

namespace SmartLock.Controllers.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message)
        : base(message)
        {
        }

        public UserNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}