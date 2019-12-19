using RestSharp;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CRMapi.Service
{
    public class Service
    {
        public string PushRatePlans(string strRatePlans, string vlToken)
        {
            var client = new RestClient(Cls.GlobalConfig.GetConfig.ServiceUrl["GetRatePlans"]);
            RestRequest restRq = new RestRequest(Method.POST);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", vlToken);
            restRq.AddParameter("undefined", strRatePlans, ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(restRq);
                string strRs = response.Content.ToString();
                
                CRMapi.Cls.LogWriter.Log("GetRatePlans", strRatePlans, strRs, null);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject jobj = JObject.Parse(strRs);
                    if (jobj["status"].ToString() == "SUCCESS")
                    {
                        return strRs;
                    }
                }                
            }
            catch (System.Net.WebException wEx)
            {
                HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
                Stream receiveStr = myRs.GetResponseStream();
                StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
                string strResponse = readStr.ReadToEnd();              
                
                CRMapi.Cls.LogWriter.Log("GetRatePlans", strRatePlans, strResponse, wEx.Message + wEx.StackTrace);               
            }
            catch (Exception ex)
            {                                
                CRMapi.Cls.LogWriter.Log("GetRatePlans", strRatePlans, null, ex.Message + ex.StackTrace);             
            }
            return null;
        }
        public string PushInventory(string strContent)
        {
            var client = new RestClient(Cls.GlobalConfig.GetConfig.ServiceUrl["SaveInventory"]);
            RestRequest restRq = new RestRequest(Method.POST);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", "???");
            restRq.AddParameter("undefined", strContent, ParameterType.RequestBody);
            var strRs = "";
            try
            {
                IRestResponse response = client.Execute(restRq);
                strRs = response.Content.ToString();
                //Log
                CRMapi.Cls.LogWriter.Log("PushInventory", strContent, strRs, null);
                return strRs;
            }
            catch (System.Net.WebException wEx)
            {
                HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
                Stream receiveStr = myRs.GetResponseStream();
                StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
                string strResponse = readStr.ReadToEnd();
                var err = new
                {
                    status = "FAIL",
                    message = strResponse
                };
                //LLog
                CRMapi.Cls.LogWriter.Log("PushInventory", strContent, strRs, wEx.Message);
                return Newtonsoft.Json.JsonConvert.SerializeObject(err);
            }
            catch (Exception ex)
            {
                var err = new
                {
                    status = "FAIL",
                    message = ex.Message + ex.StackTrace
                };
                //LLog
                return Newtonsoft.Json.JsonConvert.SerializeObject(err);
            }

        }
        public Tuple<string, string, string, string> GetCredential(string token)
        {
            RestClient client = new RestClient(Cls.GlobalConfig.GetConfig.ServiceUrl["GetCredential"]);

            RestRequest restRq = new RestRequest(Method.GET);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", token);
            string strRs = "";
            try
            {
                IRestResponse response = client.Execute(restRq);
                strRs = response.Content.ToString();


                JObject jobj = JObject.Parse(strRs);
                if (jobj["status"].ToString() != "SUCCESS") { return null; }

                string hotelId = jobj["result"]["HotelId"].ToString();
                string ChannelManagerUsername = jobj["result"]["HotelChannelUsername"].ToString();
                string ChannelManagerPassword = jobj["result"]["HotelChannelPassword"].ToString();
                string HotelAuthenticationChannelKey = jobj["result"]["HotelChannelKey"].ToString();
                return Tuple.Create(ChannelManagerUsername, ChannelManagerPassword, hotelId, HotelAuthenticationChannelKey);
            }
            catch (System.Net.WebException wEx)
            {
                HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
                Stream receiveStr = myRs.GetResponseStream();
                StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
                string strResponse = readStr.ReadToEnd();
            }
            catch (Exception ex)
            {
                //LLog
            }
            return null;
        }
        public Tuple<string, string, string, string, string> GetSaveInventory(string extranet)
        {
            string url = Cls.GlobalConfig.GetConfig.ServiceUrl["GetSaveInventory"]
                + "?" + "a=" + extranet;
            var client = new RestClient(url);
            RestRequest restRq = new RestRequest(Method.GET);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", "???");
            var strRs = "";
            try
            {
                IRestResponse response = client.Execute(restRq);
                strRs = response.Content.ToString();
                JObject jobj = JObject.Parse(strRs);
                if (jobj["status"].ToString() != "SUCCESS") { return null; }
                string body = jobj["result"][""].ToString();
                string hotelId = jobj["result"]["HotelId"].ToString();
                string channelManagerUsername = jobj["result"]["ChannelManagerUsername"].ToString();
                string channelManagerPassword = jobj["result"]["ChannelManagerPassword"].ToString();
                string hotelAuthenticationChannelKey = jobj["result"]["HotelAuthenticationChannelKey"].ToString();
                return Tuple.Create(body, channelManagerUsername, channelManagerPassword, hotelId, hotelAuthenticationChannelKey);
            }
            catch (System.Net.WebException wEx)
            {
                HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
                Stream receiveStr = myRs.GetResponseStream();
                StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
                string strResponse = readStr.ReadToEnd();

                //LLog
                //CRMapi.Cls.LogWriter.Log("GetRatePlans", strContent, strRs, wEx.Message);

            }
            catch (Exception ex)
            {
                //LLog
            }
            return null;
        }
        public string GetBookings(string extranet)
        {
            RestClient client = new RestClient(Cls.GlobalConfig.GetConfig.ServiceUrl["GetBookings"]
                + "?" + "a=" + extranet);
            RestRequest restRq = new RestRequest(Method.GET);
            restRq.AddHeader("Content-Type", "application/json");
            restRq.AddHeader("Authorization", "???");
            string strRs = "";
            try
            {
                IRestResponse response = client.Execute(restRq);
                strRs = response.Content.ToString();
                JObject jobj = JObject.Parse(strRs);
                if (jobj["status"].ToString() != "SUCCESS") { return null; }
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(response.ToString());
                return body;
            }
            catch (System.Net.WebException wEx)
            {
                HttpWebResponse myRs = (HttpWebResponse)wEx.Response;
                Stream receiveStr = myRs.GetResponseStream();
                StreamReader readStr = string.IsNullOrEmpty(myRs.CharacterSet) ? new StreamReader(receiveStr) : new StreamReader(receiveStr, System.Text.Encoding.GetEncoding(myRs.CharacterSet));
                string strResponse = readStr.ReadToEnd();

                //LLog
                //CRMapi.Cls.LogWriter.Log("GetRatePlans", strContent, strRs, wEx.Message);

            }
            catch (Exception ex)
            {
                //LLog
            }
            return null;
        }

    }
}
