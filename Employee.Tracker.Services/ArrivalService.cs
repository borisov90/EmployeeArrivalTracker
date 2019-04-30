using Employee.Tracker.Services.Contracts;
using EmployeeTracker.Data;
using EmployeeTracker.Data.Common;
using EmployeeTracker.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace EmployeeTracker.Services
{
    public class ArrivalService : IArrivalService
    {
        private string ARRIVALS_URL = "http://localhost:51396/api/clients/subscribe";
        private string CALLBACK_URL = "Home/ReceiveDataFromService";
        private const string WEB_SERVICE_TOKEN = "X-Fourth-Token";
        private const string ACCEPT_CLIENT_HEADER = "Fourth-Monitor";

        public async Task<ValidationToken> GetArrivalsFromApi(DateTime date, string baseUrl)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept-Client", ACCEPT_CLIENT_HEADER);
                var callbackAdress = baseUrl + CALLBACK_URL;
                var concatenatedURL = $"{ARRIVALS_URL}?date={date.ToString("yyyy-MM-dd")}&callback={callbackAdress}";

                ValidationToken validationToken = new ValidationToken();


                var getDataTask = await client.GetAsync(concatenatedURL).ContinueWith(async response =>
                {
                    var result = response.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var body = await result.Content.ReadAsStringAsync();

                        validationToken = (ValidationToken)JsonConvert.DeserializeObject(body, typeof(ValidationToken));
                    }
                });

                getDataTask.Wait();

                return validationToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Arrival> Parse(HttpRequestBase request)
        {
            using (var stream = new StreamReader(request.InputStream))
            {
                string requestData = stream.ReadToEnd();
                var arrivals = JsonConvert.DeserializeObject<List<Arrival>>(requestData);

                return arrivals;
            }
        }

        public bool IsValid(HttpRequestBase request, string validationToken)
        {
            bool isValid = false;

            if (request.Headers != null && request.Headers[WEB_SERVICE_TOKEN] != null
                && request.Headers[WEB_SERVICE_TOKEN] == validationToken)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}