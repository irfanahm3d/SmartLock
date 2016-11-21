/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using SmartLock.Controllers.Contracts;
using SmartLock.Controllers.Exceptions;
using SmartLock.DAL.Events;
using SmartLock.DAL.User;

namespace SmartLock.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        EventsDAL eventsDal;
        UserDAL userDal;

        JsonMediaTypeFormatter formatter;

        public EventsController()
        {
            this.eventsDal = new EventsDAL();
            this.userDal = new UserDAL();

            this.SetupJsonFormatter();
        }

        // For unit testing purposes
        internal EventsController(EventsDAL eventsDal, UserDAL userDal)
        {
            this.eventsDal = eventsDal;
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
        public HttpResponseMessage UserEvents()
        {
            var statusCode = HttpStatusCode.OK;
            var eventsResponse = new EventsResponseContract();

            try
            {
                EventsParameters parameters = 
                    EventsParameters.ParseGetEventsParameters(HttpContext.Current.Request.QueryString);

                // Check if user exists.
                this.userDal.GetUser(parameters.UserId);

                IList<EventModel> eventsList = this.eventsDal.GetUserEvents(parameters.UserId);
                eventsResponse.UserId = parameters.UserId;
                eventsResponse.EventsList = eventsResponse.ConvertToContract(eventsList);
                eventsResponse.Message = "List of user events.";
            }
            catch (InvalidParameterException paramsException)
            {
                eventsResponse.Message = paramsException.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException userException)
            {
                eventsResponse.Message = userException.Message;
            }

            var response = Request.CreateResponse(statusCode, eventsResponse, formatter);
            return response;
        }
    }
}
