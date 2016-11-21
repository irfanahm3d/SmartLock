/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using SmartLock.Controllers.Contracts;
using SmartLock.Controllers.Exceptions;
using SmartLock.DAL.User;

namespace SmartLock.Controllers
{
    public class UserController : ApiController
    {
        UserDAL userDal;

        JsonMediaTypeFormatter formatter;

        public UserController()
        {
            this.userDal = new UserDAL();

            this.SetupJsonFormatter();
        }

        // For unit testing purposes
        internal UserController(UserDAL userDal)
        {
            this.userDal = userDal;

            this.SetupJsonFormatter();
        }

        void SetupJsonFormatter()
        {
            formatter = new JsonMediaTypeFormatter();
            var json = formatter.SerializerSettings;

            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.Formatting = Newtonsoft.Json.Formatting.Indented;
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
                userResponse.Message = "User found.";
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
            
            return Request.CreateResponse(statusCode, userResponse, formatter);
        }

        // Commenting out the HttpMethod attribute so that the method is
        // inaccessible.
        ////[HttpPut]
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
            
            return Request.CreateResponse(statusCode, userResponse, formatter);
        }
    }
}
