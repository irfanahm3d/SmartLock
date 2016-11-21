/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Globalization;

namespace SmartLock.Controllers.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string parameterName)
        : base(
              String.Format(
                  CultureInfo.InvariantCulture,
                  "Parameter {0} is invalid.",
                  parameterName))
        {
        }

        public InvalidParameterException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}