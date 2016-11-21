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
    public class UserControllerTests
    {
        UserController userController;
        MockUserData mockUserData = new MockUserData();

        [TestInitialize]
        public void TestInitialize()
        {
            var mockUserDal = new UserDAL(mockUserData);

            userController = new UserController(mockUserDal);
        }

        [TestMethod]
        public void GetUserTests()
        {
            var testScenarios = new[]
            {
                new
                {
                    Name = "Email & Password (Valid/Admin) - Success",
                    QueryString = "email=aisha@test.com&password=aishatest",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedUserId = 22,
                    ExpectedUserName = "Aisha",
                    ExpectedEmail = "aisha@test.com",
                    ExpectedIsAdmin = true,
                    ExpectedMessage = "User found."
                },
                new
                {
                    Name = "Email & Password (Valid/NonAdmin) - Success",
                    QueryString = "email=sara@test.com&password=saratest",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedUserId = 21,
                    ExpectedUserName = "Sara",
                    ExpectedEmail = "sara@test.com",
                    ExpectedIsAdmin = false,
                    ExpectedMessage = "User found."
                },
                new
                {
                    Name = "Email & Password (Invalid) - Failure",
                    QueryString = "email=ryan@test.com&password=ryan",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedUserId = 0,
                    ExpectedUserName = "",
                    ExpectedEmail = "",
                    ExpectedIsAdmin = false,
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "Email (Invalid) & Password - Failure",
                    QueryString = "email=ryan@live.com&password=ryantest",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedUserId = 0,
                    ExpectedUserName = "",
                    ExpectedEmail = "",
                    ExpectedIsAdmin = false,
                    ExpectedMessage = "Not found."
                },
                new
                {
                    Name = "Email parameter invalid - Failure",
                    QueryString = "emal=ryan@test.com&password=ryantest",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedUserId = 0,
                    ExpectedUserName = "",
                    ExpectedEmail = "",
                    ExpectedIsAdmin = false,
                    ExpectedMessage = "Parameter email not found or is invalid."
                },
                new
                {
                    Name = "Password parameter invalid - Failure",
                    QueryString = "email=ryan@live.com&passwrd=ryantest",
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedUserId = 0,
                    ExpectedUserName = "",
                    ExpectedEmail = "",
                    ExpectedIsAdmin = false,
                    ExpectedMessage = "Parameter password not found or is invalid."
                },

            };

            foreach (var testScenario in testScenarios)
            {
                System.Console.WriteLine(testScenario.Name);

                userController.Request = new HttpRequestMessage();
                userController.Configuration = new HttpConfiguration();
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, "http://localhost/user", testScenario.QueryString), new HttpResponse(null));

                // Act
                var response = userController.GetUser();

                // Assert
                Assert.IsNotNull(response, "Response should not be null");
                Assert.AreEqual(testScenario.ExpectedStatusCode, response.StatusCode, "Status code is not expected");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync();
                    var userResponse = JsonConvert.DeserializeObject<UserResponseContract>(content.Result);
                    Assert.AreEqual(testScenario.ExpectedUserId, userResponse.UserId, "UserId");

                    string expectedUserName = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedUserName))
                    {
                        expectedUserName = testScenario.ExpectedUserName;
                    }
                    Assert.AreEqual(expectedUserName, userResponse.UserName, "UserName");

                    string expectedEmail = null;
                    if (!String.IsNullOrWhiteSpace(testScenario.ExpectedEmail))
                    {
                        expectedEmail = testScenario.ExpectedEmail;
                    }
                    Assert.AreEqual(expectedEmail, userResponse.Email, "Email");
                    Assert.AreEqual(testScenario.ExpectedIsAdmin, userResponse.IsAdmin, "IsAdmin");
                    Assert.AreEqual(testScenario.ExpectedMessage, userResponse.Message, "Response Message");
                }
            }
        }
    }
}
