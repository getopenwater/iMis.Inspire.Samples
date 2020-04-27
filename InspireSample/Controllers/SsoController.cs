using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using InspireSample.App;
using InspireSample.Extensions;
using JWT;

namespace InspireSample.Controllers
{
    public class SsoController : Controller
    {       

        public ActionResult Begin(string returnUrl)
        {
            Response.Cookies.Add(new HttpCookie("returnUrl", returnUrl));
            return Redirect(Constants.ImisLoginPage);
        }

        [HttpPost]
        public ActionResult Process(string refresh_token)        
        {            
            //This is the Default Return Url if none is set in the cookie
            var returnUrl = $"https://{Constants.InspireDomainName}/a/account/validatethirdpartycorporateauthresult?redirectUrl=http%3A%2F%2F{Constants.InspireDomainName}%2Fa";

            //Go back to a deep link inside Inspire if provided
            if(Request.Cookies.AllKeys.Any(c=>c == "returnUrl"))
            {
                returnUrl = Request.Cookies["returnUrl"].Value;
            }

            var iMisApi = new iMisApiHelper(Constants.ImisApiUrlBase, Constants.ImisApiClientId, Constants.ImisApiClientSecret, refresh_token);
            var token = iMisApi.GetAuthToken();
            iMisApi.AddHttpClientHeader(token);

            //get data from iMIS
            var userInfo = iMisApi.GetUsersInfo();
            var partyInfo = iMisApi.GetPartyInfo(userInfo);

            //get custom data from IQA -- REPLACE THIS PART WITH A CALL TO YOUR IQA
            //var iqaData = iMisApi.GetIqaWithParameter("$/path/to/iqa", "userId or some other data point");
            //add your code to deserialize or modify this info
            
            var additionalProfileFields = new Dictionary<string, string>();
            //additionalProfileFields.Add("[GUID OF FIELD]", "value from IQA");
            //additionalProfileFields.Add("[GUID OF FIELD]", "value");
            //additionalProfileFields.Add("[GUID OF FIELD]", "value");

            var jwtUser = new Models.JwtResultWithTimestamp
            {
                FirstName = partyInfo.PersonName.FirstName,
                LastName = partyInfo.PersonName.LastName,
                Email = !partyInfo.Emails.FakeValues.Where(email => email.IsPrimary == true).First().Address.IsNullOrEmpty() ? partyInfo.Emails.FakeValues.Where(email => email.IsPrimary == true).First().Address : (!partyInfo.Emails.FakeValues.First().Address.IsNullOrEmpty() ? partyInfo.Emails.FakeValues.First().Address : ""),        
                UserData = userInfo.Items.FakeValues.First().Party.PartyId.ToString(),
                UserIsMember = true,
                UserNameExists = true,
                UserValidatedSuccessfully = true,                
                Company = partyInfo.PrimaryOrganization.Name,
                Website = partyInfo.WebsiteUrl,
                ThirdPartyUniqueId = userInfo.Items.FakeValues.First().Party.PartyId.ToString(),
            };

            //This is what maps your custom fields to sign in user fields
            jwtUser.ProfileTextFieldData = additionalProfileFields;

            var jwt = JWT.JsonWebToken.Encode(jwtUser, Constants.SharedSecret, JwtHashAlgorithm.HS256);
            return Redirect($"{returnUrl}&token={jwt}");
        }
    }
}