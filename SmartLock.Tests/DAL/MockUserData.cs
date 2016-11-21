/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.Controllers.Exceptions;
using SmartLock.DAL.User;
using SmartLock.Models;

namespace SmartLock.Tests.DAL
{
    public class MockUserData : IUserData
    {
        readonly IDictionary<int, UserInfo> UserData = new Dictionary<int, UserInfo>
        {
            {
                20,
                new UserInfo
                {
                    UserId = 20,
                    UserName = "Ryan",
                    Email = "ryan@test.com",
                    Password = "ryantest",
                    IsAdmin = true
                }
            },
            {
                21,
                new UserInfo
                {
                    UserId = 21,
                    UserName = "Sara",
                    Email = "sara@test.com",
                    Password = "saratest",
                    IsAdmin = false
                }
            },
            {
                22,
                new UserInfo
                {
                    UserId = 22,
                    UserName = "Aisha",
                    Email = "aisha@test.com",
                    Password = "aishatest",
                    IsAdmin = true
                }
            },
        };

        public int CreateUser(string userName, string userEmail, string userPassword, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(int userId)
        {
            UserInfo userInfo;
            if (!this.UserData.TryGetValue(userId, out userInfo))
            {
                throw new UserNotFoundException("Not found.");
            }

            return new UserModel
            {
                UserId = userInfo.UserId,
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                IsAdmin = userInfo.IsAdmin
            };
        }

        public UserModel GetUser(string userEmail, string userPassword)
        {
            UserInfo userInfo = this.UserData.Values.SingleOrDefault(
                v => 
                    String.Equals(v.Email, userEmail, StringComparison.Ordinal) &&
                    String.Equals(v.Password, userPassword, StringComparison.Ordinal));

            if (userInfo == null)
            {
                throw new UserNotFoundException("Not found.");
            }

            return new UserModel
            {
                UserId = userInfo.UserId,
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                IsAdmin = userInfo.IsAdmin
            };
        }
    }
}
