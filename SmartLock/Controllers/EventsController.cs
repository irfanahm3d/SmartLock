using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SmartLock.Controllers
{
    public class EventsController : ApiController
    {
        [HttpGet]
        public JsonResult<string> UserEvents()
        {
            return Json("events");
        }
    }
}
