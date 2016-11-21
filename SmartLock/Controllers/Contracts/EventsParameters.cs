/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Specialized;
using SmartLock.Controllers.Exceptions;

namespace SmartLock.Controllers.Contracts
{
    public class EventsParameters
    {
        public int UserId { get; set; }

        public static EventsParameters ParseGetEventsParameters(NameValueCollection queryParameters)
        {
            int userId = 0;
            if (!Int32.TryParse(queryParameters["userId"], out userId))
            {
                throw new InvalidParameterException("userId");
            }

            return new EventsParameters
            {
                UserId = userId
            };
        }
    }
}