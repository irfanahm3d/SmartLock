/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.User
{
    public interface IUserData
    {
        void CreateUser(string userName, string userEmail, string userPassword, bool isAdmin);

        string GetUser(string userEmail, string userPassword);

        string GetUser(int userId);
    }
}
