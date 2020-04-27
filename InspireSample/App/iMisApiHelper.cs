using InspireSample.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace InspireSample.App
{
    public class iMisApiHelper : IDisposable
    {
        private string _iMIS_CLIENT_ID;
        private string _iMIS_CLIENT_SECRET;
        private string _refreshToken;
        private string _baseUrl;
        private HttpClient _client;
        private AuthTokenResult _authResponse;

        public iMisApiHelper(string baseUrl, string clientID, string clientSecret, string refreshToken)
        {
            _iMIS_CLIENT_ID = clientID;
            _iMIS_CLIENT_SECRET = clientSecret;
            _refreshToken = refreshToken;
            _baseUrl = baseUrl;
            _client = new HttpClient();
            _authResponse = new AuthTokenResult();
        }

        public string GetAuthToken()
        {
            var tokenDict = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", _refreshToken },
                { "client_id", _iMIS_CLIENT_ID },
                { "client_secret", _iMIS_CLIENT_SECRET }
            };

            _authResponse = _client.PostForm(_baseUrl + "/token", tokenDict).ReadContentAs<AuthTokenResult>();

            var token = _authResponse.AccessToken;

            return token;
        }

        public void AddHttpClientHeader(string token)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
        }

        public iMisUserResultClasses.IMisUserResult GetUsersInfo()
        {
            var userInfoString = _client.Get(_baseUrl + $"/api/user?username={_authResponse.UserName}").ReadContentAsString();
            userInfoString = userInfoString.Replace("$values", "fakeValues").Replace("$type", "fakeType");
            var userInfo = userInfoString.FromJson<iMisUserResultClasses.IMisUserResult>();

            return userInfo;
        }

        public iMisPartyResultClasses.IMisPartyResult GetPartyInfo(iMisUserResultClasses.IMisUserResult userInfo)
        {
            var partyInfoString = _client.Get(_baseUrl + $"/api/party/{userInfo.Items.FakeValues.First().Party.PartyId}").ReadContentAsString();
            partyInfoString = partyInfoString.Replace("$values", "fakeValues").Replace("$type", "fakeType").Replace("$value", "fakeValue");
            var partyInfo = partyInfoString.FromJson<iMisPartyResultClasses.IMisPartyResult>();

            return partyInfo;
        }

        public string GetIqaWithParameter(string queryPath, string parameter)
        {
            return _client.Get(_baseUrl + $"/api/iqa?QueryName={queryPath}&parameter=eq:{parameter}").ReadContentAsString();
            
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }


    #region Models

    public class AuthTokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("as:client_id")]
        public string AsClientId { get; set; }

        [JsonProperty(".issued")]
        public string Issued { get; set; }

        [JsonProperty(".expires")]
        public string Expires { get; set; }
    }

    public class iMisPartyQueryClasses
    {
        public partial class IMisPartyQueryResult
        {
            public string FakeType { get; set; }
            public Items Items { get; set; }
            public long Offset { get; set; }
            public long Limit { get; set; }
            public long Count { get; set; }
            public long TotalCount { get; set; }
            public object NextPageLink { get; set; }
            public bool HasNext { get; set; }
            public long NextOffset { get; set; }
        }

        public partial class Items
        {
            public string FakeType { get; set; }
            public ItemsFakeValue[] FakeValues { get; set; }
        }

        public partial class ItemsFakeValue
        {
            public string FakeType { get; set; }
            public long EntityTypeName { get; set; }
            public Properties Properties { get; set; }
        }

        public partial class Properties
        {
            public string FakeType { get; set; }
            public PropertiesFakeValue[] FakeValues { get; set; }
        }

        public partial class PropertiesFakeValue
        {
            public string FakeType { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }

    public class iMisPartyResultClasses
    {
        public partial class IMisPartyResult
        {
            public string FakeType { get; set; }
            public PersonName PersonName { get; set; }
            public PrimaryOrganization PrimaryOrganization { get; set; }
            public AdditionalAttributes AdditionalAttributes { get; set; }
            public Addresses Addresses { get; set; }
            public AlternateIds AlternateIds { get; set; }
            public Emails Emails { get; set; }
            public FinancialInformation FinancialInformation { get; set; }
            public Phones Phones { get; set; }
            public Salutations Salutations { get; set; }
            public AdditionalAttributes SocialNetworks { get; set; }
            public AdditionalAttributes CommunicationTypePreferences { get; set; }
            public bool SortIsOverridden { get; set; }
            public UpdateInformation UpdateInformation { get; set; }
            public string WebsiteUrl { get; set; }
            public long PartyId { get; set; }
            public long Id { get; set; }
            public Guid UniformId { get; set; }
            public Status Status { get; set; }
            public string Name { get; set; }
            public string Sort { get; set; }
        }

        public partial class AdditionalAttributes
        {
            public string FakeType { get; set; }
            public List<AdditionalAttributesFakeValue> FakeValues { get; set; }
        }

        public partial class AdditionalAttributesFakeValue
        {
            public string FakeType { get; set; }
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public partial class ValueClass
        {
            public string FakeType { get; set; }
            public bool FakeValue { get; set; }
        }

        public partial class Addresses
        {
            public string FakeType { get; set; }
            public List<AddressesFakeValue> FakeValues { get; set; }
        }

        public partial class AddressesFakeValue
        {
            public string FakeType { get; set; }
            public AdditionalAttributes AdditionalLines { get; set; }
            public Address Address { get; set; }
            public string AddresseeText { get; set; }
            public string AddressPurpose { get; set; }
            public CommunicationPreferences CommunicationPreferences { get; set; }
            public string Email { get; set; }
            public long FullAddressId { get; set; }
            public string Phone { get; set; }
            public FakeValue Salutation { get; set; }
            public string DisplayName { get; set; }
            public string DisplayOrganizationName { get; set; }
        }

        public partial class Address
        {
            public string FakeType { get; set; }
            public long AddressId { get; set; }
            public AddressLines AddressLines { get; set; }
            public string Barcode { get; set; }
            public string CityName { get; set; }
            public string CountrySubEntityCode { get; set; }
            public string FullAddress { get; set; }
            public string PostalCode { get; set; }
            public long VerificationStatus { get; set; }
        }

        public partial class AddressLines
        {
            public string FakeType { get; set; }
            public List<string> FakeValues { get; set; }
        }

        public partial class CommunicationPreferences
        {
            public string FakeType { get; set; }
            public List<CommunicationPreferencesFakeValue> FakeValues { get; set; }
        }

        public partial class CommunicationPreferencesFakeValue
        {
            public string FakeType { get; set; }
            public string Reason { get; set; }
        }

        public partial class FakeValue
        {
            public string FakeType { get; set; }
            public SalutationMethod SalutationMethod { get; set; }
            public string Text { get; set; }
            public string SalutationId { get; set; }
        }

        public partial class SalutationMethod
        {
            public string FakeType { get; set; }
            public string PartySalutationMethodId { get; set; }
        }

        public partial class AlternateIds
        {
            public string FakeType { get; set; }
            public List<AlternateIdsFakeValue> FakeValues { get; set; }
        }

        public partial class AlternateIdsFakeValue
        {
            public string FakeType { get; set; }
            public string IdType { get; set; }
        }

        public partial class Emails
        {
            public string FakeType { get; set; }
            public List<EmailsFakeValue> FakeValues { get; set; }
        }

        public partial class EmailsFakeValue
        {
            public string FakeType { get; set; }
            public string Address { get; set; }
            public string EmailType { get; set; }
            public bool? IsPrimary { get; set; }
        }

        public partial class FinancialInformation
        {
            public string FakeType { get; set; }
        }

        public partial class PersonName
        {
            public string FakeType { get; set; }
            public string FirstName { get; set; }
            public string InformalName { get; set; }
            public string LastName { get; set; }
            public string NameSuffix { get; set; }
            public string FullName { get; set; }
        }

        public partial class Phones
        {
            public string FakeType { get; set; }
            public List<PhonesFakeValue> FakeValues { get; set; }
        }

        public partial class PhonesFakeValue
        {
            public string FakeType { get; set; }
            public string Number { get; set; }
            public string PhoneType { get; set; }
        }

        public partial class PrimaryOrganization
        {
            public string FakeType { get; set; }
            public long OrganizationPartyId { get; set; }
            public string Name { get; set; }
        }

        public partial class Salutations
        {
            public string FakeType { get; set; }
            public List<FakeValue> FakeValues { get; set; }
        }

        public partial class Status
        {
            public string FakeType { get; set; }
            public string PartyStatusId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public partial class UpdateInformation
        {
            public string FakeType { get; set; }
            public string CreatedBy { get; set; }
            public string UpdatedBy { get; set; }
        }

        public partial struct ValueUnion
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }

    public class iMisUserResultClasses
    {
        public partial class IMisUserResult
        {
            public string FakeType { get; set; }
            public Items Items { get; set; }
            public long Offset { get; set; }
            public long Limit { get; set; }
            public long Count { get; set; }
            public long TotalCount { get; set; }
            public object NextPageLink { get; set; }
            public bool HasNext { get; set; }
            public long NextOffset { get; set; }
        }

        public partial class Items
        {
            public string FakeType { get; set; }
            public List<FakeValue> FakeValues { get; set; }
        }

        public partial class FakeValue
        {
            public string FakeType { get; set; }
            public DateTimeOffset EffectiveDate { get; set; }
            public DateTimeOffset ExpirationDate { get; set; }
            public bool IsDisable { get; set; }
            public object Roles { get; set; }
            public bool IsAnonymous { get; set; }
            public long UserId { get; set; }
            public string UserName { get; set; }
            public Party Party { get; set; }
        }

        public partial class Party
        {
            public string FakeType { get; set; }
            public string CityName { get; set; }
            public string CountryName { get; set; }
            public string CountrySubEntityName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public long PartyId { get; set; }
            public long Id { get; set; }
            public Guid UniformId { get; set; }
            public Status Status { get; set; }
            public string Name { get; set; }
            public string Sort { get; set; }
            public bool IsMarkedForDelete { get; set; }
        }

        public partial class Status
        {
            public string FakeType { get; set; }
            public string PartyStatusId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }

    #endregion

}