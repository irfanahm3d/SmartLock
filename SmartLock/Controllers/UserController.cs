/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using SmartLock.DAL.User;

namespace SmartLock.Controllers
{
    public class UserController : ApiController
    {
        UserDAL userDal;

        public UserController()
        {
            this.userDal = new UserDAL();
        }

        // For unit testing purposes
        internal UserController(UserDAL userDal)
        {
            this.userDal = userDal;
        }

        [HttpGet]
        public JsonResult<string> GetUser([FromUri]string userEmail, [FromUri]string userPassword)
        {
            string result = String.Empty;
            try
            {
                result = this.userDal.GetUser(userEmail, userPassword);
            }
            catch (Exception)
            {
                result = "Not found";
            }

            return Json(result);
        }

        [HttpPost]
        void CreateUser([FromUri]string userName, [FromUri]string userEmail, [FromUri]string userPassword, [FromUri]bool isAdmin)
        {
            this.userDal.CreateUser(userName, userEmail, userPassword, isAdmin);
        }
    }
}
