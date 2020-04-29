using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspireSample.Controllers
{
    public class LookupController : Controller
    {        
        [HttpPost]
        public ActionResult Validate(ValidateRequest req)
        {
            if (req.value == "allowed")
                return Json(new 
                {
                    isValid = true,
                    message = ""
                });
            else
                return Json(new 
                {
                    isValid = false,
                    message = "Invalid Message"
                });
        }
            public ActionResult Find(string query = "")
        {            

            var fakeDb = FakeLookupDatabse();
            var matches = fakeDb.Where(item => item.FullName.ToLower().Contains(query.ToLower())).ToArray();

            var data = new List<LookupItem>();
            foreach (var matchingItem in matches)
            {
                data.Add(
                    new LookupItem
                    {
                        value = matchingItem.FullName,
                        label = matchingItem.FullName
                    }
                );
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Fill(string value)
        {
            var fakeDb = FakeLookupDatabse();
            var fakeItem = fakeDb.Where(c => c.FullName == value).First();

            var prefillResponse = new List<PrefillResponseItem>();

            var prefillResponseItem = new PrefillResponseItem 
            {
                tableAlias = null,
                fields = new List<PrefillItem>()
            };

            //Map these results to the openwater field names
            prefillResponseItem.fields.Add(new PrefillItem
            {
                alias = "firstName",
                value = fakeItem.FirstName
            });

            prefillResponseItem.fields.Add(new PrefillItem
            {
                alias = "lastName",
                value = fakeItem.LastName
            });

            prefillResponseItem.fields.Add(new PrefillItem
            {
                alias = "university",
                value = fakeItem.University
            });

            prefillResponse.Add(prefillResponseItem);

            return Json(prefillResponse, JsonRequestBehavior.AllowGet);
        }


        public class FakeData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string University { get; set; }
            public string FullName
            {
                get { return FirstName + " " + LastName; }
            }
        }

        public class LookupItem
        {
            public string label { get; set; }
            public string value { get; set; }
        }

        public class PrefillItem
        {
            public string alias { get; set; }
            public string value { get; set; }
        }

        public class PrefillResponseItem
        {
            public string tableAlias { get; set; }
            public List<PrefillItem> fields { get; set; }
        }

        public class ValidateRequest
        {
            public string value { get; set; }
        }

        private static List<FakeData> FakeLookupDatabse()
        {
            var database = new List<FakeData>()
            {
                new FakeData { FirstName = "John",     LastName = "Doe",        University = "Louisiana State University"},
                new FakeData { FirstName = "Jane",     LastName = "Doe",        University = "California State University"},
                new FakeData { FirstName = "Bill",     LastName = "Gates",      University = "Lakeside School"},
                new FakeData { FirstName = "Bill",     LastName = "Nye",        University = "Cornell University"},
                new FakeData { FirstName = "Bill",     LastName = "Skarsgard",  University = "Sodra Latin"},
                new FakeData { FirstName = "Mark",     LastName = "Zuckerberg", University = "Harvard"},
                new FakeData { FirstName = "Elon",     LastName = "Musk",       University = "University of Pennsylvania"},
                new FakeData { FirstName = "Oprah",    LastName = "Winfrey",    University = "Tennessee State University"},
                new FakeData { FirstName = "Michelle", LastName = "Obama",      University = "Harvard"},
            };

            return database;
        }
    }
}