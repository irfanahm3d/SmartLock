/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */
 
namespace SmartLock.DAL.User
{
    public class UserDAL
    {
        IUserData userDataLayer;

        public UserDAL()
        {
            this.userDataLayer = new UserData();
        }

        // unit testing purposes
        internal UserDAL(IUserData userData)
        {
            this.userDataLayer = userData;
        }

        public void CreateUser(string userName, string userEmail, string userPassword, bool isAdmin)
        {
            this.userDataLayer.CreateUser(userName, userEmail, userPassword, isAdmin);
        }

        public string GetUser(string userEmail, string userPassword)
        {
            return this.userDataLayer.GetUser(userEmail, userPassword);
        }

        public string GetUser(int userId)
        {
            return this.userDataLayer.GetUser(userId);
        }
    }
}