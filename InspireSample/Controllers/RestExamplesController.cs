using InspireSample.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspireSample.Controllers
{
    public class RestExamplesController : Controller
    {
        // GET: RestExamples
        public ActionResult Index()
        {
            var api = new OpenWater.ApiClient.OpenWaterApiClient(Constants.InspireDomainName, Constants.InspireApiKey);

            var twentyFourHoursAgo = DateTime.UtcNow.AddDays(-1);
            var applicationData = api.GetApplications(lastModifiedSinceUtc: twentyFourHoursAgo);
            return Json(applicationData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ApplicationById(int id)
        {
            var api = new OpenWater.ApiClient.OpenWaterApiClient(Constants.InspireDomainName, Constants.InspireApiKey);

            var applicationData = api.GetApplicationById(id);
            return Json(applicationData, JsonRequestBehavior.AllowGet);
        }
    }
}