/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */
/*
* SmartLock
* Copyright (c) Irfan Ahmed. 2016
*/

using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SmartLock.Models;
using SmartLock.Controllers.Exceptions;

namespace SmartLock.DAL.User
{
    public class UserData : IUserData
    {
        public int CreateUser(string userName, string userEmail, string userPassword, bool isAdmin)
        {
            var hashedPassword = this.GetSHA256Password(userPassword);
            using (var smartLock = new SmartLockEntities())
            {
                var userInfo = new UserInfo
                {
                    UserName = userName,
                    Email = userEmail,
                    Password = hashedPassword,
                    IsAdmin = isAdmin
                };

                smartLock.UserInfoes.Add(userInfo);
                smartLock.SaveChanges();

                return userInfo.UserId;
            }
        }

        public UserModel GetUser(string userEmail, string userPassword)
        {
            var hashedPassword = this.GetSHA256Password(userPassword);
            using (var smartLock = new SmartLockEntities())
            {
                var query = from users in smartLock.UserInfoes
                            where (users.Email == userEmail && users.Password == hashedPassword)
                            select users;

                UserInfo user = query.FirstOrDefault();
                if (user == null)
                {
                    throw new UserNotFoundException("Not found.");
                }

                return new UserModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin
                };
            }
        }

        public UserModel GetUser(int userId)
        {
            using (var smartLock = new SmartLockEntities())
            {
                var query = from users in smartLock.UserInfoes
                            where users.UserId == userId
                            select users;

                UserInfo user = query.FirstOrDefault();

                if (user == null)
                {
                    throw new UserNotFoundException("Not found.");
                }

                return new UserModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    IsAdmin = user.IsAdmin
                };
            }
        }

        string GetSHA256Password(string password)
        {
            var hashedPassword = new StringBuilder();

            var hasher = SHA256.Create();
            byte[] hashedArray = hasher.ComputeHash(Encoding.ASCII.GetBytes(password));
            for (int i = 0; i < hashedArray.Length; i++)
            {
                hashedPassword.Append(hashedArray[i].ToString("x2"));
            }

            return hashedPassword.ToString();
        }
    }
}