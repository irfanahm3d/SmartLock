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

namespace SmartLock.DAL.User
{
    public class UserData : IUserData
    {
        public void CreateUser(string userName, string userEmail, string userPassword, bool isAdmin)
        {
            var hashedPassword = this.GetSHA256Password(userPassword);
            using (var smartLock = new SmartLockEntities())
            {
                smartLock.UserInfoes.Add(
                    new UserInfo
                    {
                        UserName = userName,
                        Email = userEmail,
                        Password = hashedPassword,
                        IsAdmin = isAdmin
                    });
                smartLock.SaveChanges();
            }
        }

        public string GetUser(string userEmail, string userPassword)
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
                    throw new ArgumentException("user");
                }


                return String.Format(
                    CultureInfo.InvariantCulture, "{0};{1};{2}", user.UserId, user.UserName, user.IsAdmin);
            }
        }

        public string GetUser(int userId)
        {
            using (var smartLock = new SmartLockEntities())
            {
                var query = from users in smartLock.UserInfoes
                            where users.UserId == userId
                            select users;

                UserInfo user = query.FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("userId");
                }

                return String.Format(
                    CultureInfo.InvariantCulture, "{0};{1};{2}", user.UserId, user.UserName, user.IsAdmin);
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