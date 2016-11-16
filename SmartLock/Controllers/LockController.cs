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

        // GET lock/5
        [HttpGet]
        public JsonResult<string> GetLockState(int id)
        {
            return Json(lockDal.GetLockState(id));
        }

        // PUT lock/5?value=5
        [HttpPut]
        public JsonResult<string> ModifyLockState(int id, [FromUri]string state)
        {
            // authenticate user before attempting to modify lock.
            return Json(lockDal.ModifyLockState(id, state));
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
