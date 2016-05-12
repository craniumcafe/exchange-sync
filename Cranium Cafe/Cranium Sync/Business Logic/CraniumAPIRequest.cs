using CraniumCafeSync.Models;
using Redemption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CraniumCafeSync.Business_Logic
{
    class CraniumAPIRequest
    {
        public const int POST = 0;
        public const int PUT = 1;
        public const int DELETE = 2;
        private static CraniumConfig CraniumConfig = new CraniumConfig();
        private static UserDetails UserDetails = new UserDetails();

        public CraniumAPIRequest(CraniumConfig craniumConfig, UserDetails ud)
        {
            CraniumConfig = craniumConfig;
            UserDetails = ud;
        }

        public void ValidateUser(BackgroundWorker bw)
        {
            string strLoginJson = "username_or_email=" + "CcGlobals.a" + "&password=" + "CcGlobals.b";
            SendJsonOverHttp(strLoginJson, "login", "POST");
        }

        public bool AddEvent(Appointment appointment)
        {
            try
            {
                SendJsonOverHttp(new JsonBodyCreator().GetEventJson(appointment), "events", "PUT");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in AddEvent:- " + ex.Message);
                return false;
            }
        }

        public bool UpdateEvent(Appointment appointment)
        {
            try
            {
                SendJsonOverHttp(new JsonBodyCreator().GetEventJson(appointment), "events", "PUT");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in UpdateEvent:- " + ex.Message);
                return false;
            }
        }

        public bool DeleteEvent(Appointment appointment)
        {
            try
            {
                return SendJsonOverHttp(new JsonBodyCreator().GetEventJson(appointment), "events", "DELETE");
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in DeleteEvent:- " + ex.Message);
                return false;
            }
        }

        public bool UpdateMeeting(string strMeetingId, Appointment appointment)
        {
            try
            {
                SendJsonOverHttp(new JsonBodyCreator().GetMeetingJson(strMeetingId, appointment), "meetings", "PUT");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in UpdateMeeting:- " + ex.Message);
                return false;
            }
        }

        public bool AddOfficeHour(Appointment appointment)
        {
            try
            {
                SendJsonOverHttp(new JsonBodyCreator().GetOfficeHourJson(appointment), "officehours", "PUT");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in AddOfficeHour:- " + ex.Message);
                return false;
            }
        }

        public bool UpdateOfficeHour(Appointment appointment)
        {
            try
            {
                SendJsonOverHttp(new JsonBodyCreator().GetOfficeHourJson(appointment), "officehours", "PUT");
                return true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in UpdateOfficeHour:- " + ex.Message);
                return false;
            }
        }

        public bool DeleteOfficeHour(Appointment appointment)
        {
            try
            {
                return SendJsonOverHttp(new JsonBodyCreator().GetOfficeHourJson(appointment), "officehours", "DELETE");
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in DeleteOfficeHour:- " + ex.Message);
                return false;
            }
        }

        private bool SendJsonOverHttp(string strEventsJson, string strAction, string strMethod)
        {
            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create("https://api.craniumcafe.com/rest/v1/" + strAction);
                request.Method = strMethod;
                string postData = strEventsJson;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                // request.Headers.Add("auth-key", "davton_test");               
                request.Headers.Add("auth-key", new EnDecrypt().Decrypt(CraniumConfig.RestKey));
                request.Headers.Add("auth-token", GenerateAuthToken());

                Stream postStream = request.GetRequestStream();
                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                if (GetJsonValue(responseFromServer, "success") == "true")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SendJsonOverHttp:- " + ex.Message);
                return false;
            }
        }

        public string GetJsonValue(string strData, string strKey)
        {
            try
            {
                string strPattern = "\"" + strKey + "\":(.*?[^,}])[,}]";
                Match match = Regex.Match(strData, strPattern, RegexOptions.IgnoreCase);

                if (match.Groups.Count > 0)
                {
                    string strResult = match.Groups[1].Value.ToString().Trim();
                    if (strResult == "")
                    {
                        return "false";
                    }
                    else
                    {
                        return strResult;
                    }
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetJsonValue:- " + ex.Message);
                return "false";
            }

        }

        public string GenerateAuthToken()
        {
            try
            {             
                string timestamp = UnixTimeNow();
                Random rnd = new Random();
                string rand = rnd.Next(1000000000).ToString();
                //string secretSalt = "vofyp67m9i19m37qj33cuhkdpe3banif3qv77e5ma0g75235z94d6hqepi2q2d6fegq1xmmbcflz3j3aexyium9pg5tg5s75kk4o";
                string secretSalt = new EnDecrypt().Decrypt(CraniumConfig.RestSecret);

                string authToken = timestamp + ":" + rand + ":" + UserDetails.UserID + ":" + CalculateSHA1(timestamp + ":" + rand + ":" + UserDetails.UserID + ":" + secretSalt);
                return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authToken));
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GenerateAuthToken:- " + ex.Message);
                return "";
            }

        }

        private string UnixTimeNow()
        {
            try
            {
                DateTime date = DateTime.UtcNow;
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64((date - epoch).TotalSeconds).ToString();
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in UnixTimeNow:- " + ex.Message);
                return "";
            }

        }

        private string CalculateSHA1(string text)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(text);
                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in CalculateSHA1:- " + ex.Message);
                return "";
            }

        }

    }

}
