/*
 * SmartLock
 * Copyright (c) Irfan Ahmed. 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SmartLock.Controllers;
using SmartLock.Controllers.Contracts;
using SmartLock.DAL.Events;
using SmartLock.DAL.Lock;
using SmartLock.DAL.User;
using SmartLock.Tests.DAL;

namespace SmartLock.Tests
{
    [TestClass]
    public class LockControllerTests
    {
        LockController lockController;
        MockEventsData mockEventsData = new MockEventsData();

        [TestInitialize]
        public void TestInitialize()
        {
            var mockLockDal = new LockDAL(new MockLockData());
            var mockEventsDal = new EventsDAL(mockEventsData);
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
                    ExpectedLockId = 30,
                    ExpectedUserId = 20,
                    ExpectedLockState = "Locked",
                    ExpectedMessage = "State of lock."
                },
                new
                {
                    Name = "Lock not found - Failure",
                    QueryString = "lockId=35&userId=20",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 35,
                    ExpectedUserId = 20,
                    ExpectedLockState = "",
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "lockId=30&userId=25",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 30,
                    ExpectedUserId = 25,
                    ExpectedLockState = "",
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "LockId parameter invalid - Failure",
                    QueryString = "lockd=30&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 20,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockId not found or is invalid."
                },
                new
                {
                    Name = "LockId parameter value invalid - Failure",
                    QueryString = "lockId=fiveforty&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockId not found or is invalid."
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "lockId=30&userd=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter userId not found or is invalid."
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=Ryan",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter userId not found or is invalid."
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
                Assert.IsNotNull(response, "Response should not be null");
                Assert.AreEqual(testScenario.ExpectedStatusCode, response.StatusCode, "Status code is not expected");
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync();
                    var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(content.Result);
                    Assert.AreEqual(testScenario.ExpectedLockId, lockResponse.LockId, "LockId");
                    Assert.AreEqual(testScenario.ExpectedUserId, lockResponse.UserId, "UserId");

                    string expectedLockState = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedLockState))
                    {
                        expectedLockState = testScenario.ExpectedLockState;
                    }
                    Assert.AreEqual(expectedLockState, lockResponse.LockState, "LockState");
                    Assert.AreEqual(testScenario.ExpectedMessage, lockResponse.Message, "Response Message");
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
                    Name = "Authorized user modifies lock (Unlock) - Success",
                    QueryString = "lockId=32&userId=21&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 32,
                    ExpectedUserId = 21,
                    ExpectedLockState = "Unlock",
                    ExpectedMessage = "Door Unlocked successfully."
                },
                new
                {
                    Name = "Authorized user modifies lock (Lock) - Success",
                    QueryString = "lockId=30&userId=22&lockState=lock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 30,
                    ExpectedUserId = 22,
                    ExpectedLockState = "Lock",
                    ExpectedMessage = "Door Locked successfully."
                },
                new
                {
                    Name = "Unauthorized user modifies lock - Failure",
                    QueryString = "lockId=30&userId=21&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.Unauthorized,
                    ExpectedLockId = 30,
                    ExpectedUserId = 21,
                    ExpectedLockState = "",
                    ExpectedMessage = "User unauthorized."
                },
                new
                {
                    Name = "Lock not found - Failure",
                    QueryString = "lockId=35&userId=20&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 35,
                    ExpectedUserId = 20,
                    ExpectedLockState = "",
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "lockId=30&userId=25&lockState=unlock",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 30,
                    ExpectedUserId = 25,
                    ExpectedLockState = "",
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "LockId parameter invalid - Failure",
                    QueryString = "lockd=30&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockId not found or is invalid."
                },
                new
                {
                    Name = "LockId parameter value invalid - Failure",
                    QueryString = "lockId=fiveforty&userId=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockId not found or is invalid."
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "lockId=30&userd=20",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter userId not found or is invalid."
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=Ryan",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter userId not found or is invalid."
                },
                new
                {
                    Name = "LockState parameter invalid - Failure",
                    QueryString = "lockId=30&userId=20&locksate=lock",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockState not found or is invalid."
                },
                new
                {
                    Name = "LockState parameter value invalid - Failure",
                    QueryString = "lockId=30&userId=20&lockState=break",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockState = "",
                    ExpectedMessage = "Parameter lockState not found or is invalid."
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
                    var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(content.Result);
                    Assert.AreEqual(testScenario.ExpectedLockId, lockResponse.LockId, "LockId");
                    Assert.AreEqual(testScenario.ExpectedUserId, lockResponse.UserId, "UserId");

                    string expectedLockState = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedLockState))
                    {
                        expectedLockState = testScenario.ExpectedLockState;

                        // check if the corresponding user event was added.
                        EventModel userEvent = 
                            mockEventsData.GetUserEvents(testScenario.ExpectedUserId).SingleOrDefault();

                        Assert.AreEqual(testScenario.ExpectedLockId, userEvent.LockId, "Event LockId");
                        Assert.AreEqual(testScenario.ExpectedLockState, userEvent.State, "Event LockState");
                    }
                    Assert.AreEqual(expectedLockState, lockResponse.LockState, "LockState");
                    Assert.AreEqual(testScenario.ExpectedMessage, lockResponse.Message, "Response Message");
                }
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
                    QueryString = "userId=20&lockName=Conf+1&allowedUsers=20,21,22",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 33,
                    ExpectedUserId = 20,
                    ExpectedLockName = "Conf 1",
                    ExpectedLockState = "Locked",
                    ExpectedAllowedUsers = new List<int> { 20, 21, 22 },
                    ExpectedMessage = "Lock created successfully."
                },
                new
                {
                    Name = "Unauthorized user creates lock - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.Unauthorized,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "User unauthorized."
                },
                new
                {
                    Name = "User not found - Failure",
                    QueryString = "userId=25&lockName=Main+door&allowedUsers=25",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "UserId parameter invalid - Failure",
                    QueryString = "userd=21&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter userId not found or is invalid."
                },
                new
                {
                    Name = "UserId parameter value invalid - Failure",
                    QueryString = "userId=Ryan&lockName=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter userId not found or is invalid."
                },
                new
                {
                    Name = "LockName parameter invalid - Failure",
                    QueryString = "userId=21&lockNme=Main+door&allowedUsers=21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter lockName not found or is invalid."
                },
                new
                {
                    Name = "AllowedUsers parameter invalid - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowUsers=21,23",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter allowedUsers not found or is invalid."
                },
                new
                {
                    Name = "AllowedUsers parameter value invalid (type) - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=ryan,sara",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter allowedUsers not found or is invalid."
                },
                new
                {
                    Name = "AllowedUsers parameter values invalid (delimiter) - Failure",
                    QueryString = "userId=21&lockName=Main+door&allowedUsers=20;21",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedLockId = 0,
                    ExpectedUserId = 0,
                    ExpectedLockName = "",
                    ExpectedLockState = "",
                    ExpectedAllowedUsers = new List<int>(),
                    ExpectedMessage = "Parameter allowedUsers not found or is invalid."
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
                    var lockResponse = JsonConvert.DeserializeObject<LockResponseContract>(content.Result);
                    Assert.AreEqual(testScenario.ExpectedLockId, lockResponse.LockId, "LockId");
                    Assert.AreEqual(testScenario.ExpectedUserId, lockResponse.UserId, "UserId");

                    string expectedLockState = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedLockState))
                    {
                        expectedLockState = testScenario.ExpectedLockState;
                    }
                    Assert.AreEqual(expectedLockState, lockResponse.LockState, "LockState");

                    string expectedLockName = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedLockName))
                    {
                        expectedLockName = testScenario.ExpectedLockName;
                    }
                    Assert.AreEqual(expectedLockName, lockResponse.LockName, "LockName");

                    IList<int> expectedAllowedUsers = null;
                    if (testScenario.ExpectedAllowedUsers.Count > 0)
                    {
                        expectedAllowedUsers = testScenario.ExpectedAllowedUsers;
                        CollectionAssert.AreEqual(
                            expectedAllowedUsers.ToList(),
                            lockResponse.AllowedUsers.ToList(),
                            "AllowedUsers");
                    }
                    else
                    {
                        Assert.IsNull(lockResponse.AllowedUsers, "AllowedUsers should be null.");
                    }
                    Assert.AreEqual(testScenario.ExpectedMessage, lockResponse.Message, "Response Message");
                }
            }
        }
    }
}
