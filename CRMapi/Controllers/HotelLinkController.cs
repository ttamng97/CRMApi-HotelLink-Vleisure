using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CRMapi.Controllers
{
    [Route("api/[Action]")]
    [ApiController]
    public class HotelLinkController : ControllerBase
    {
        private IMemoryCache cache;
        public HotelLinkController(IOptionsSnapshot<Model.Config> config, IMemoryCache cache)
        {
            Cls.GlobalConfig.SetCOnfig(config.Value);
            this.cache = cache;
        }
     
        private string GetAuth(string auth)
        {
            string tokenaccess = "";
            try
            {
                if (cache.TryGetValue(auth, out tokenaccess)) { return tokenaccess; }

                var client = new RestClient(Cls.GlobalConfig.GetConfig.HotelLinkUrls["oAuth"]);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", "Basic " + auth);
                request.AddParameter("grant_type", "client_credentials");
                IRestResponse response = client.Execute(request);
                string rs = response.Content.ToString();
                Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(rs);
                if (!jobj["result"].Value<bool>()) { return ""; }
                tokenaccess = jobj["data"]["access_token"].ToString();
                int expire = jobj["data"]["expires_in"].Value<int>();
                this.cache.Set(auth, tokenaccess, TimeSpan.FromSeconds(expire));
            }
            catch { }
            return tokenaccess;
        }
      
      
        [HttpGet]
        public Model.Response GetRatePlanExtranet()
        {
            string vlToken = Request.Headers["Authorization"];
            Model.Response result = new Model.Response { result = false, message = "", data = null };

            //step1:Get Credential
            Service.Service sv = new Service.Service();
            Tuple<string, string, string, string> hotelCredential = sv.GetCredential(vlToken);
            if(hotelCredential == null) {
                result.message = "Can not get hotelmap";    
                return result; 
            }
            //Access Token
            string hotelLinkToken = GetAuth(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes((hotelCredential.Item1+ ":" + hotelCredential.Item2))));
            if (string.IsNullOrEmpty(hotelLinkToken))
            {
                result.message = "Can not get token oAuth";
                return result;
            }
            //step2:Get rate plan from hotelLink
            Service.HotelLink hotelLink = new Service.HotelLink();
            string hotelLinkRs = hotelLink.GetRatePlan(hotelCredential, hotelLinkToken);
            if (string.IsNullOrEmpty(hotelLinkRs)) {
                result.message = "Can not GetRatePlan";
                return result;
            }

            result.data = hotelLinkRs;
            //step3:Push rate plan to service
            string svRs = sv.PushRatePlans(hotelLinkRs, vlToken);
            if (string.IsNullOrEmpty(svRs)) { result.message = "Can not PushRatePlans "; return result; }
            result.result = true;            
            return result;
        }

        //[HttpPost]
        //public Model.Response SaveInventory([FromBody]Model.GetSaveInventory rq)
        //{
        //    Model.Response result = new Model.Response { result = false, message = "", data = null };
        //    //Access Token
        //    string auth = GetAuth(Request.Headers["auth"]);
        //    if (auth == null)
        //    {
        //        result.message = "Can not get token oAuth";
        //        return result;
        //    }
        //    //step1:Get ... map
        //    Service.Service sv = new Service.Service();
        //    Tuple<string,string,string,string,string> inventory = sv.GetSaveInventory(rq.hotelId);
        //    if (inventory == null)
        //    {
        //        result.message = "Can not get inventory";
        //        return result;
        //    }
        //    //step2:Get SaveInventory from hotelLink
        //    Service.HotelLink hotelLink = new Service.HotelLink();
        //    string inventoryRs = hotelLink.GetSaveInventory(inventory, auth);
        //    if (inventoryRs == null)
        //    {
        //        result.message = "Can not GetSaveInventory";
        //        return result;
        //    }
        //    //step3:Push rate plan to service
        //    string svRs = sv.PushInventory(inventoryRs);
        //    if (svRs == null) { result.message = "Can not PushRatePlans "; return result; }
        //    Cls.LogWriter.Log("GetSaveInventory", inventoryRs, svRs, null);
        //    result.result = true;
        //    return result;
        //}

       
        //public Model.Response GetBookings([FromBody]Model.GetBookings rq)
        //{
        //    Model.Response result = new Model.Response { result = false, message = "", data = null };
        //    //Access Token
        //    string auth = GetAuth(Request.Headers["auth"]);
        //    if (auth == null)
        //    {
        //        result.message = "Can not get token oAuth";
        //        return result;
        //    }
        //    //step1:Get ... map
        //    Service.Service sv = new Service.Service();
        //    string inventory = sv.GetBookings(rq.a);
        //    if (inventory == null)
        //    {
        //        result.message = "Can not GetBookings";
        //        return result;
        //    }
        //    //step2:Get SaveInventory from hotelLink
        //    Service.HotelLink hotelLink = new Service.HotelLink();
        //    string inventoryRs = hotelLink.GetBookings(inventory, auth);
        //    if (inventoryRs == null)
        //    {
        //        result.message = "Can not GetBookings";
        //        return result;
        //    }
        //    //step3:Push rate plan to service
        //    string svRs = sv.PushRatePlans(inventoryRs);
        //    if (svRs == null) { result.message = "Can not GetBookings "; }
        //    Cls.LogWriter.Log("GetBookings", inventoryRs, svRs, null);
        //    result.result = true;
        //    return result;
        //}
    }
}