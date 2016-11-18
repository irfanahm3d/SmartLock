/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

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
        public JsonResult<List<string>> GetLocksState()
        {
            return Json(this.lockDal.GetLocksState().ToList());
        }

        // GET lock?lockId=5
        [HttpGet]
        public JsonResult<string> GetLockState([FromUri]int lockId)
        {
            return Json(this.lockDal.GetLockState(lockId));
        }

        // PUT lock?lockId=5&state=Unlock&userId=12101
        [HttpPost]
        public JsonResult<string> ModifyLockState(int lockId, [FromUri]int userId, [FromUri]string state)
        {
            string resultString = string.Empty;
            // authenticate user before attempting to modify lock.
            if (this.lockDal.ModifyLockState(lockId, userId, state))
            {
                resultString = state + "ed";
            }

            return Json(resultString);
        }

        // POST lock
        // Note: Only Admins can utilize this api
        [HttpPost]
        public void CreateLock([FromUri]string lockName, [FromUri]IList<int> allowedUsers)
        {
            this.lockDal.CreateLock(lockName, allowedUsers);
        }

        // DELETE lock/5
        // Note: Only Admins can utilize this api
        public void Delete(int id)
        {
        }
    }
}
