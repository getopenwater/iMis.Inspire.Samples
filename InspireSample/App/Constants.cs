using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspireSample.App
{
    public static class Constants
    {
        //Platform Constants
        public const string InspireApiKey = "00000000-0000-0000-0000-000000000000"; //https://help.getopenwater.com/en/articles/3110614-openwater-rest-api
        public const string InspireDomainName = "YOURDOMAIN.imis-inspire.com"; //example: demo.secure-platform.com or demo.imis-inspire.com

        //SSO Specific
        public const string SharedSecret = "YOUR-JWT-SECRET";

        public const string ImisLoginPage = "https://domain.imiscloud.com/path-to/Custom-Login.aspx";
        public const string ImisApiUrlBase = "https://domain.imiscloud.com/iMisService"; //don't add the /token

        public const string ImisApiClientId = "Imis-Client-Id";
        public const string ImisApiClientSecret = "Imis-Client-Secret";

        //Lookup Widget Specific
        public const string IqaName = "$/path/to/your/iqa";
    }
}