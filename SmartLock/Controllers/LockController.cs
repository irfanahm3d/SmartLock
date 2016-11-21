﻿/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using SmartLock.Controllers.Contracts;
using SmartLock.Controllers.Exceptions;
using SmartLock.DAL.Events;
using SmartLock.DAL.Lock;
using SmartLock.DAL.User;
using System.Globalization;

namespace SmartLock.Controllers
{
    public class LockController : ApiController
    {
        LockDAL lockDal;
        UserDAL userDal;
        EventsDAL eventsDal;

        public LockController()
        {
            this.lockDal = new LockDAL();
            this.eventsDal = new EventsDAL();
            this.userDal = new UserDAL();
        }

        // For unit testing purposes
        internal LockController(LockDAL lockDal, EventsDAL eventsDal, UserDAL userDal)
        {
            this.lockDal = lockDal;
            this.eventsDal = eventsDal;
            this.userDal = userDal;
        }

        // GET lock/
        //[HttpGet]
        ////public JsonResult<List<string>> GetLocks()
        ////{
        ////    return Json(this.lockDal.GetLocks().ToList());
        ////}

        // GET lock?lockId=5
        [HttpGet]
        public HttpResponseMessage GetLockState()
        {
            var statusCode = HttpStatusCode.OK;
            var lockResponse = new LockResponseContract();

            try
            {
                LockParameters parameters = LockParameters.ParseGetLockParameters(HttpContext.Current.Request.QueryString);
                lockResponse.LockId = parameters.LockId;
                lockResponse.UserId = parameters.UserId;

                // Check to see if the user exists.
                this.userDal.GetUser(parameters.UserId);

                lockResponse.LockState = this.lockDal.GetLockState(parameters.LockId);
                lockResponse.Message = "State of lock.";
            }
            catch (InvalidParameterException paramException)
            {
                lockResponse.Message = paramException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            // We do not want to give information on what input is incorrect. 
            // Keeping the response generic so that malicious intent is not
            // provided with too much information.
            catch (UserNotFoundException userException)
            {
                lockResponse.Message = userException.Message;
            }
            catch (LockNotFoundException lockException)
            {
                lockResponse.Message = lockException.Message;
            }
                        
            var response = Request.CreateResponse(statusCode, JsonConvert.SerializeObject(lockResponse));
            return response;
        }

        // PUT lock?lockId=5&state=Unlock&userId=12101
        [HttpPost]
        public HttpResponseMessage ModifyLockState()
        {
            bool? result = null;
            LockParameters parameters = null;
            var statusCode = HttpStatusCode.OK;
            var lockResponse = new LockResponseContract();

            try
            {
                parameters = LockParameters.ParsePostLockParameters(HttpContext.Current.Request.QueryString);
                lockResponse.LockId = parameters.LockId;
                lockResponse.UserId = parameters.UserId;

                // Check to see if the user exists.
                this.userDal.GetUser(parameters.UserId);
                
                result = this.lockDal.ModifyLockState(parameters.LockId, parameters.UserId, parameters.LockState);
                lockResponse.LockState = result.Value ? parameters.LockState : "Failed";

                lockResponse.Message = result.Value ?
                    String.Format(CultureInfo.InvariantCulture, "Door {0}ed successfully.", parameters.LockState) :
                    String.Format(CultureInfo.InvariantCulture, "Door {0} failed.", parameters.LockState); 
            }
            catch (InvalidParameterException paramException)
            {
                lockResponse.Message = paramException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException userException)
            {
                lockResponse.Message = userException.Message;
            }
            catch (UnauthorizedUserException userAuthException)
            {
                lockResponse.Message = userAuthException.Message;
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (LockNotFoundException lockException)
            {
                lockResponse.Message = lockException.Message;
            }
            finally
            {
                if (result != null)
                {
                    this.eventsDal.CreateEvent(parameters.LockId, parameters.UserId, lockResponse.LockState);
                }
            }

            var response = Request.CreateResponse(statusCode, JsonConvert.SerializeObject(lockResponse));
            return response;
        }

        // PUT lock
        // Note: Only Admins can utilize this api
        [HttpPut]
        public HttpResponseMessage CreateLock()
        {
            var statusCode = HttpStatusCode.OK;
            var lockResponse = new LockResponseContract();
            string result = String.Empty;
            
            try
            {
                LockParameters parameters = LockParameters.ParsePutLockParameters(HttpContext.Current.Request.QueryString);
                lockResponse.UserId = parameters.UserId;

                // Check if the user exists.
                UserModel user = this.userDal.GetUser(parameters.UserId);

                // Check if users specified in access list exist.
                foreach (int allowedUser in parameters.AllowedUsers)
                {
                    this.userDal.GetUser(allowedUser);
                }

                // If the user is an admin they can create a lock with an access list.
                if (user.IsAdmin)
                {
                    LockModel lockModel = this.lockDal.CreateLock(parameters.LockName, parameters.AllowedUsers);
                    if (lockModel == null)
                    {
                        lockResponse.Message = "Failed to create the lock.";
                    }

                    lockResponse.Message = "Lock created successfully.";
                }
                else
                {
                    lockResponse.Message = "User unauthorized.";
                    statusCode = HttpStatusCode.Unauthorized;
                }
            }
            catch (InvalidParameterException paramException)
            {
                lockResponse.Message = paramException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException userException)
            {
                lockResponse.Message = userException.Message;
            }

            var response = Request.CreateResponse(statusCode, JsonConvert.SerializeObject(lockResponse));
            return response;
        }

        // DELETE lock/5
        // Note: Only Admins can utilize this api
        public void Delete(int id)
        {
        }
    }
}
