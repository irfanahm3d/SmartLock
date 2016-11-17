using SmartLock.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace SmartLock.Controllers
{
    public class LockController : ApiController
    {
        LockDAL lockDal;

        public LockController()
        {
            lockDal = new LockDAL();
        }

        // For unit testing purposes
        internal LockController(LockDAL dal)
        {
            lockDal = dal;
        }

        // GET lock/
        [HttpGet]
        public JsonResult<List<string>> GetLocksState()
        {
            return Json(lockDal.GetLocksState().ToList());
        }

        // GET lock?lockId=5
        [HttpGet]
        public JsonResult<string> GetLockState([FromUri]int lockId)
        {
            return Json(lockDal.GetLockState(lockId));
        }

        // PUT lock?lockId=5&state=Unlock&userId=12101
        [HttpPost]
        public JsonResult<string> ModifyLockState(int lockId, [FromUri]int userId, [FromUri]string state)
        {
            string resultString = string.Empty;
            // authenticate user before attempting to modify lock.
            if (lockDal.ModifyLockState(lockId, userId, state))
            {
                resultString = state + "ed";
            }

            return Json(resultString);
        }

        // POST lock
        // Note: Only Admins can utilize this api
        [HttpPost]
        public void CreateLock([FromBody]string value)
        {
        }

        // DELETE lock/5
        // Note: Only Admins can utilize this api
        public void Delete(int id)
        {
        }
    }
}
