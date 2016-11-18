/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using SmartLock.DAL.Events;
using SmartLock.DAL.Lock;

namespace SmartLock.Controllers
{
    public class LockController : ApiController
    {
        LockDAL lockDal;
        EventsDAL eventsDal;

        public LockController()
        {
            this.lockDal = new LockDAL();
            this.eventsDal = new EventsDAL();
        }

        // For unit testing purposes
        internal LockController(LockDAL lockDal, EventsDAL eventsDal)
        {
            this.lockDal = lockDal;
            this.eventsDal = eventsDal;
        }

        // GET lock/
        [HttpGet]
        public JsonResult<List<string>> GetLocks()
        {
            return Json(this.lockDal.GetLocks().ToList());
        }

        // GET lock?lockId=5
        [HttpGet]
        public JsonResult<string> GetLockState([FromUri]int lockId)
        {
            string result = String.Empty;
            try
            {
                result = this.lockDal.GetLockState(lockId);
            }
            catch (Exception)
            {
                result = "Not found";
            }

            return Json(result);
        }

        // PUT lock?lockId=5&state=Unlock&userId=12101
        [HttpPost]
        public JsonResult<string> ModifyLockState(int lockId, [FromUri]int userId, [FromUri]string state)
        {
            bool result = false;
            string finalState = String.Empty;
            
            try
            {
                result = this.lockDal.ModifyLockState(lockId, userId, state);
                finalState = result ? state : "Failed";
            }
            catch (Exception)
            {
                finalState = "Unauthorized";
            }
            finally
            {
                this.eventsDal.CreateEvent(lockId, userId, finalState);
            }

            return Json(finalState);
        }

        // POST lock
        // Note: Only Admins can utilize this api
        [HttpPost]
        public JsonResult<string> CreateLock([FromUri]string lockName, [FromUri]int currentUserId, [FromUri]IList<int> allowedUsers)
        {
            string result = String.Empty;
            try
            {
                result = 
                    this.lockDal.CreateLock(lockName, currentUserId, allowedUsers) ? 
                    "Created" : "Failed";
            }
            catch (Exception)
            {
                result = "Unauthorized";
            }

            return Json(result);
        }

        // DELETE lock/5
        // Note: Only Admins can utilize this api
        public void Delete(int id)
        {
        }
    }
}
