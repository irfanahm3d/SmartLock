/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using SmartLock.Controllers.Contracts;
using SmartLock.Controllers.Exceptions;
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
        public HttpResponseMessage GetUser()
        {
            var statusCode = HttpStatusCode.OK;
            var userResponse = new UserResponseContract();

            try
            {
                UserParameters parameters = 
                    UserParameters.ParseGetUserParameters(HttpContext.Current.Request.QueryString);

                UserModel user = this.userDal.GetUser(parameters.Email, parameters.Password);
                userResponse.UserId = user.UserId;
                userResponse.UserName = user.UserName;
                userResponse.Email = user.Email;
                userResponse.IsAdmin = user.IsAdmin;
            }
            catch (InvalidParameterException paramException)
            {
                userResponse.Message = paramException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException userException)
            {
                userResponse.Message = userException.Message;
            }

            var response = Request.CreateResponse(statusCode, JsonConvert.SerializeObject(userResponse));
            return response;
        }

        [HttpPut]
        HttpResponseMessage CreateUser()
        {
            var statusCode = HttpStatusCode.OK;
            var userResponse = new UserResponseContract();

            try
            {
                UserParameters parameters = 
                    UserParameters.ParsePutUserParameters(HttpContext.Current.Request.QueryString);

                int userId = this.userDal.CreateUser(
                    parameters.UserName,
                    parameters.Email,
                    parameters.Password,
                    parameters.IsAdmin);

                userResponse.UserName = parameters.UserName;
                userResponse.Email = parameters.Email;
                userResponse.UserId = userId;
                userResponse.IsAdmin = parameters.IsAdmin;
            }
            catch (InvalidParameterException paramException)
            {
                userResponse.Message = paramException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }

            var response = Request.CreateResponse(statusCode, JsonConvert.SerializeObject(userResponse));
            return response;
        }
    }
}
