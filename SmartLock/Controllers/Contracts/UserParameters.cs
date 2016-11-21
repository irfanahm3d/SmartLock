/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Specialized;
using SmartLock.Controllers.Exceptions;

namespace SmartLock.Controllers.Contracts
{
    public class UserParameters
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
        
        public static UserParameters ParseGetUserParameters(NameValueCollection queryParameters)
        {
            string email = queryParameters["email"];
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new InvalidParameterException("email");
            }

            string password = queryParameters["password"];
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new InvalidParameterException("password");
            }

            return new UserParameters
            {
                Email = email,
                Password = password
            };
        }

        public static UserParameters ParsePutUserParameters(NameValueCollection queryParameters)
        {
            string userName = queryParameters["userName"];
            if (String.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidParameterException("userName");
            }

            string email = queryParameters["email"];
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new InvalidParameterException("email");
            }

            string password = queryParameters["password"];
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new InvalidParameterException("password");
            }

            bool isAdmin = false;
            if (!Boolean.TryParse(queryParameters["isAdmin"], out isAdmin))
            {
                throw new InvalidParameterException("isAdmin");
            }

            return new UserParameters
            {
                UserName = userName,
                Email = email,
                Password = password,
                IsAdmin = isAdmin
            };
        }
    }
}