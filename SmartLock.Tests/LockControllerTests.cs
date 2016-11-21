/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartLock.Controllers;
using SmartLock.DAL.Lock;
using SmartLock.Tests.DAL;
using SmartLock.DAL.Events;
using SmartLock.DAL.User;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Net;
using SmartLock.Controllers.Contracts;
using Newtonsoft.Json;

namespace SmartLock.Tests
{
    [TestClass]
    public class LockControllerTests
    {
        LockController lockController;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockLockDal = new LockDAL(new MockLockData());
            var mockEventsDal = new EventsDAL(new MockEventsData());
            var mockUserDal = new UserDAL(new MockUserData());

            lockController = new LockController(mockLockDal, mockEventsDal, mockUserDal);
        }

        [TestMethod]
        public void GetLockTests()
        {
            var testScenarios = new []
            {
                new
                {
                    Name = "LockId and UserId - Success",
                    QueryString = "lockId=30&userId=20",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = "Locked"
                },
                new
                {
                    Name = "Lock not found - Failure",
                    QueryString = "lockId=35&userId=20",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "lockId=30&userId=25",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockId parameter invalid - Failure",
                    QueryString = "lockd=30&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockId parameter value invalid - Failure",
                    QueryString = "lockId=fiveforty&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "lockId=30&userd=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=Ryan",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },

            };

            foreach (var testScenario in testScenarios)
            {
                System.Console.WriteLine(testScenario.Name);
                
                lockController.Request = new HttpRequestMessage();
                lockController.Configuration = new HttpConfiguration();
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, "http://localhost/lock", testScenario.QueryString), new HttpResponse(null));

                // Act
                var response = lockController.GetLockState();

                // Assert
                Assert.IsNotNull(response, "Response should not be null.");
                Assert.AreEqual(testScenario.ExpectedStatusCode, response.StatusCode, "Status code is not expected.");
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync();
                    ////var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(
                    ////    content.Result,
                    ////    new JsonSerializerSettings
                    ////    {
                    ////        NullValueHandling = NullValueHandling.Ignore
                    ////    });
                    ////Assert.AreEqual(testScenario.ExpectedLockState, lockResponse.LockState);
                }
            }
        }

        [TestMethod]
        public void ModifyLockTests()
        {
            var testScenarios = new[]
            {
                new
                {
                    Name = "Authorized user modifies lock - Success",
                    QueryString = "lockId=30&userId=20&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = "Unlocked"
                },
                new
                {
                    Name = "Unauthorized user modifies lock - Failure",
                    QueryString = "lockId=30&userId=21&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.Unauthorized,
                    ExpectedLockState = "Unauthorized"
                },
                new
                {
                    Name = "Lock not found - Failure",
                    QueryString = "lockId=35&userId=20&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "lockId=30&userId=25&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockId parameter invalid - Failure",
                    QueryString = "lockd=30&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockId parameter value invalid - Failure",
                    QueryString = "lockId=fiveforty&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "lockId=30&userd=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=Ryan",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockState parameter invalid - Failure",
                    QueryString = "lockId=30&userId=20&locksate=lock",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockState parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=20&lockState=break",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },

            };

            foreach (var testScenario in testScenarios)
            {
                System.Console.WriteLine(testScenario.Name);

                lockController.Request = new HttpRequestMessage();
                lockController.Configuration = new HttpConfiguration();
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, "http://localhost/lock", testScenario.QueryString), new HttpResponse(null));

                // Act
                var response = lockController.ModifyLockState();

                // Assert
                Assert.IsNotNull(response, "Response should not be null.");
                Assert.AreEqual(testScenario.ExpectedStatusCode, response.StatusCode, "Status code is not expected.");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync();
                    ////var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(
                    ////    content.Result,
                    ////    new JsonSerializerSettings
                    ////    {
                    ////        NullValueHandling = NullValueHandling.Ignore
                    ////    });
                    ////Assert.AreEqual(testScenario.ExpectedLockState, lockResponse.LockState);
                }

                // check created event as well
            }
        }

        [TestMethod]
        public void CreateLockTests()
        {
            var testScenarios = new[]
            {
                new
                {
                    Name = "Authorized user creates lock - Success",
                    QueryString = "userId=20&lockName=Main+door&allowedUsers=20,21,22",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = "Unlocked"
                },
                new
                {
                    Name = "Unauthorized user creates lock - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.Unauthorized,
                    ExpectedLockState = "Unauthorized"
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "userId=25&lockName=Main+door&allowedUsers=25",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "userd=21&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "userId=Ryan&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "LockName parameter invalid - Failure",
                    QueryString = "userId=21&lockNme=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "AllowedUsers parameter invalid - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowUsers=21,23",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "AllowedUsers parameter value invalid - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=ryan,sara",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },
                new
                {
                    Name = "AllowedUsers parameter values invalid - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=20;21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockState = ""
                },

            };

            foreach (var testScenario in testScenarios)
            {
                System.Console.WriteLine(testScenario.Name);

                lockController.Request = new HttpRequestMessage();
                lockController.Configuration = new HttpConfiguration();
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, "http://localhost/lock", testScenario.QueryString), new HttpResponse(null));

                // Act
                var response = lockController.CreateLock();

                // Assert
                Assert.IsNotNull(response, "Response should not be null.");
                Assert.AreEqual(testScenario.ExpectedStatusCode, response.StatusCode, "Status code is not expected.");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync();
                    ////var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(
                    ////    content.Result,
                    ////    new JsonSerializerSettings
                    ////    {
                    ////        NullValueHandling = NullValueHandling.Ignore
                    ////    });
                    ////Assert.AreEqual(testScenario.ExpectedLockState, lockResponse.LockState);
                }
            }
        }
    }
}
