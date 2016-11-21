/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

namespace SmartLock.DAL.User
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}