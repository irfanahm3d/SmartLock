/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using SmartLock.DAL.Events;

namespace SmartLock.Controllers
{
    public class EventsController : ApiController
    {
        EventsDAL eventsDal;

        public EventsController()
        {
            this.eventsDal = new EventsDAL();
        }

        // For unit testing purposes
        internal EventsController(EventsDAL eventsDal)
        {
            this.eventsDal = eventsDal;
        }
        [HttpGet]
        public JsonResult<IList<string>> UserEvents([FromUri]int userId)
        {
            return Json(this.eventsDal.GetUserEvents(userId));
        }
    }
}
