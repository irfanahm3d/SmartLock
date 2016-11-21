/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

namespace SmartLock.DAL.User
{
    interface IUserData
    {
        int CreateUser(string userName, string userEmail, string userPassword, bool isAdmin);

        UserModel GetUser(string userEmail, string userPassword);

        UserModel GetUser(int userId);
    }
}
