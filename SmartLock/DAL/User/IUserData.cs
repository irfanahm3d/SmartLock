/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;

namespace SmartLock.DAL.User
{
    public interface IUserData
    {
        int CreateUser(string userName, string userEmail, string userPassword, bool isAdmin);

        UserModel GetUser(string userEmail, string userPassword);

        UserModel GetUser(int userId);
    }
}
