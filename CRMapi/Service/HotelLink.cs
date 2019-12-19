using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CRMapi.Service
{
    public class HotelLink
    {
        public string GetRatePlan(Tuple<string, string, string, string> rqInfo, string auth)
        {
            var jsonRq = new
            {
                Lang = "en",
                Credential = new
                {
                    ChannelManagerUsername = rqInfo.Item1,
                    ChannelManagerPassword = rqInfo.Item2,
                    HotelId = rqInfo.Item3,
                    HotelAuthenticationChannelKey = rqInfo.Item4,
                }
            };
            string strRq = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRq);
            var client = new RestClient(Cls.GlobalConfig.GetConfig.HotelLinkUrls["GetRatePlans"]);

            RestRequest restRq = new RestRequest(Method.POST);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", "Bearer " + auth);
            restRq.AddParameter("undefined", strRq, ParameterType.RequestBody);
            var strRs = "";
            try
            {
                IRestResponse response = client.Execute(restRq);
                strRs = response.Content.ToString();
                CRMapi.Cls.LogWriter.Log("GetRatePlans", strRq, strRs, null);
                return strRs;
            }
            catch (System.Net.WebException wEx)
            {
                CRMapi.Cls.LogWriter.Log("GetRatePlans", strRq, strRs, wEx.Message + wEx.StackTrace );
                return null;
            }
        }
        //public string GetSaveInventory( Tuple<string,string,string,string,string> rqInfo, string auth)
        //{
        //    var client = new RestClient(Cls.GlobalConfig.GetConfig.HotelLinkUrls["SaveInventory"]);
        //    var jsonRq = new
        //    {
        //        Inventories = new { rqInfo.Item1 },
        //        Lang = "en",
        //        Credential = new
        //        {
        //            ChannelManagerUsername = rqInfo.Item2,
        //            ChannelManagerPassword = rqInfo.Item3,
        //            HotelId = rqInfo.Item4,
        //            HotelAuthenticationChannelKey = rqInfo.Item5,
        //        }
        //    };
        //    string Auth = auth;
        //    string strRq = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRq);
        //    RestRequest restRq = new RestRequest(Method.POST);
        //    restRq.AddHeader("Content-Type", "application/json");
        //    restRq.AddHeader("Authorization", "Bearer " + Auth);
        //    restRq.AddParameter("undefined", strRq, ParameterType.RequestBody);
        //    var strRs = "";
        //    try
        //    {
        //        IRestResponse response = client.Execute(restRq);
        //        strRs = response.Content.ToString();
        //        CRMapi.Cls.LogWriter.Log("GetSaveInventory", strRq, strRs, null);
        //        return strRs;
        //    }
        //    catch (System.Net.WebException wEx)
        //    {
        //        HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
        //        Stream receiveStr = myRs.GetResponseStream();
        //        StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
        //        string strResponse = readStr.ReadToEnd();
        //        CRMapi.Cls.LogWriter.Log("GetSaveInventory", strRq, strRs, wEx.Message + wEx.StackTrace);
        //        return null;
        //    }
        //}
        //public string GetBookings(string rqInfo, string auth)
        //{
        //    string strRq = Newtonsoft.Json.JsonConvert.SerializeObject(rqInfo);
        //    var client = new RestClient(Cls.GlobalConfig.GetConfig.HotelLinkUrls["GetBookings"]);
        //    string Auth = auth;
        //    RestRequest restRq = new RestRequest(Method.POST);
        //    restRq.AddHeader("Content-Type", "application/json");
        //    restRq.AddHeader("Authorization", "Bearer " + Auth);
        //    restRq.AddParameter("undefined", strRq, ParameterType.RequestBody);
        //    var strRs = "";
        //    try
        //    {
        //        IRestResponse response = client.Execute(restRq);
        //        strRs = response.Content.ToString();
        //        CRMapi.Cls.LogWriter.Log("GetRatePlans", strRq, strRs, null);
        //        return strRs;
        //    }
        //    catch (System.Net.WebException wEx)
        //    {
        //        CRMapi.Cls.LogWriter.Log("GetRatePlans", strRq, strRs, wEx.Message + wEx.StackTrace);
        //        return null;
        //    }
        //}
    }
}
