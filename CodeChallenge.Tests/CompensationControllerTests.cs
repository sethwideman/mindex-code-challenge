using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = 85000.00M,
                EffectiveDate = DateTime.Parse("01/01/2020")
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;
            var responseCompensation = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(employeeId, responseCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, responseCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, responseCompensation.EffectiveDate);

        }

        [TestMethod]
        public void GetCurrentCompensation_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = 105000.00M,
                EffectiveDate = DateTime.Parse("05/01/2020")
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json")).Wait();

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}/compensation/current");
            var response = getRequestTask.Result;
            var responseCompensation = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, responseCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, responseCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, responseCompensation.EffectiveDate);
        }
    }
}
