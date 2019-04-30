using System;
using NUnit.Framework;
using Moq;
using Employee.Tracker.Services.Contracts;
using EmployeeTracker.Services;
using System.Threading.Tasks;
using EmployeeTracker.Data;
using EmployeeTracker.Data.Repositories;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Employee.Tracker.Tests
{
    [TestFixture]
    public class ArrivalsTests
    {
        private Mock<IArrivalRepository> _arrivalsRepoMock;
        private Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        private IArrivalService arrivalService = new ArrivalService();
        private Mock<IArrivalService> _mockedArrivalService = new Mock<IArrivalService>();
        private Arrival newArrival = new Arrival() { EmployeeId = 1, Id = 1, When = DateTime.Now };
        private List<Arrival> arrivals = new List<Arrival>();
        private ValidationToken token = new ValidationToken(){ Token = "token", Expires = DateTime.Now.AddDays(1) };
        private DateTime today = DateTime.Now;
        private readonly string testUrlAdress = "http://fakeBaseUrl";

        [SetUp]
        public void SetUp()
        {
            _mockedArrivalService = new Mock<IArrivalService>();
            _mockedArrivalService.Setup(mas => mas.GetArrivalsFromApi(today, testUrlAdress)).Returns(Task.FromResult(token));

            _arrivalsRepoMock = new Mock<IArrivalRepository>();
            _arrivalsRepoMock.Setup(arm => arm.Add(newArrival));
            _arrivalsRepoMock.Setup(arm => arm.Delete(newArrival));
            _arrivalsRepoMock.Setup(arm => arm.All());
            _arrivalsRepoMock.Setup(arm => arm.Find(s => s.Id == newArrival.Id));
            _arrivalsRepoMock.Setup(arm => arm.Get(newArrival.Id));

            request.SetupGet(x => x.Headers).Returns((System.Collections.Specialized.NameValueCollection)null);

            arrivals.Add(newArrival);
            arrivals.Add(newArrival);
        }

        [Test]
        public void ArrivalsRepo_Adds_Arrival()
        {
            _arrivalsRepoMock.Object.Add(newArrival);
            _arrivalsRepoMock.Verify(x => x.Add(newArrival));
        }

        [Test]
        public void ArrivalsRepo_ReturnsAll_Arrivals()
        {
            _arrivalsRepoMock.Object.All();
            _arrivalsRepoMock.Verify(x => x.All());
        }

        [Test]
        public void ArrivalsRepo_Delete_Arrival()
        {
            _arrivalsRepoMock.Object.Delete(newArrival);
            _arrivalsRepoMock.Verify(x => x.Delete(newArrival));
        }

        [Test]
        public void ArrivalsRepo_Find_Arrival()
        {
            _arrivalsRepoMock.Object.Find(s=>s.Id == newArrival.Id);
            _arrivalsRepoMock.Verify(x => x.Find(s => s.Id == newArrival.Id));
        }

        [Test]
        public void ArrivalsRepo_Get_Arrival()
        {
            _arrivalsRepoMock.Object.Get(newArrival.Id);
            _arrivalsRepoMock.Verify(x => x.Get(newArrival.Id));
        }

        [Test]
        public void ArrivalsService_ValidationIfNoHeaders_ReturnsFalse()
        {
            var valid = arrivalService.IsValid(request.Object, token.Token);

            Assert.That(valid, Is.False);
        }

        [Test]
        public void ArrivalsService_ValidationIfCorrectHeaders_ReturnsTrue()
        {
            var headers = new System.Collections.Specialized.NameValueCollection() { };
            headers.Add("X-Fourth-Token", "token");

            request.SetupGet(x => x.Headers).Returns(headers);

            var valid = arrivalService.IsValid(request.Object, token.Token);

            Assert.That(valid, Is.True);
        }

        [Test]
        public void ArrivalsService_ValidationIfWrongHeaders_ReturnsFalse()
        {

            var headers = new System.Collections.Specialized.NameValueCollection() { };
            headers.Add("X-Fourth-Token", "mistakentoken");

            var valid = arrivalService.IsValid(request.Object, token.Token);

            Assert.That(valid, Is.False);
        }

        [Test]
        public void ArrivalsService_Parse_ShouldNotBeNull()
        {
            var arrivalsAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(arrivals);

            IEnumerable<Arrival> parsedArrivals = null;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(arrivalsAsJson)))
            {
                request.SetupGet(x => x.InputStream).Returns(stream);

                parsedArrivals = arrivalService.Parse(request.Object);
            }

            Assert.IsNotNull(parsedArrivals);
        }

        [Test]
        public void ArrivalsService_ParseWrongData_ShouldReturnNull()
        {
            var emptyArrivalsList = new List<Arrival>();
            var arrivalsAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(emptyArrivalsList);

            IEnumerable<Arrival> parsedArrivals = null;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(arrivalsAsJson)))
            {
                request.SetupGet(x => x.InputStream).Returns(stream);

                parsedArrivals = arrivalService.Parse(request.Object);
            }

            Assert.IsEmpty(parsedArrivals);
        }

        [Test]
        public void ArrivalsService_GetArrivalsFromApi_ShouldReturnToken()
        {
            Task<ValidationToken> result = _mockedArrivalService.Object.GetArrivalsFromApi(today, testUrlAdress);

            ValidationToken validationToken = result.Result;

            Assert.IsNotNull(validationToken.Token);
        }
    }
}
