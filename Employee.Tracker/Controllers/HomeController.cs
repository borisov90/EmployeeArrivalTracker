using Employee.Tracker.Services.Contracts;
using EmployeeTracker.Data.Common;
using EmployeeTracker.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmployeeTracker.Controllers
{
    public class HomeController : Controller
    {
        private IArrivalService _arrivalService;
        private IUnitOfWork _unitOfWork;

        public HomeController(IArrivalService arrivalService, IUnitOfWork unitOfWork)
        {
            _arrivalService = arrivalService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var currentUrl = Request.Url.AbsoluteUri;

            DateTime today = DateTime.Now;

            try
            {
                var validationToken = await _arrivalService.GetArrivalsFromApi(today, currentUrl);
                if (validationToken != null)
                {
                    HttpContext.Application["ValidationToken"] = validationToken.Token;
                }
                return ArrivalsView();
            }
            catch (Exception)
            {
                //maybe implement some logging later
                return View("Error");
            }
        }

        public async Task<HttpResponseMessage> ReceiveDataFromService()
        {
            var webServiceToken = (string)HttpContext.Application["ValidationToken"];

            bool valid = _arrivalService.IsValid(Request, webServiceToken);

            if (valid)
            {
                try
                {
                    var arrivals = _arrivalService.Parse(Request);

                    foreach (var arrival in arrivals)
                    {
                        _unitOfWork.Arrivals.Add(arrival);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    //maybe implement some logging later
                }
            }

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }

        public ActionResult ArrivalsView()
        {
            var arrivals = this._unitOfWork.Arrivals.All();
            ArrivalViewModel arrivalsViewModel = new ArrivalViewModel() { Arrivals = arrivals};
            return View("Arrivals", arrivalsViewModel);
        }
    }
}