using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CraniumCafeConfig.Business_Logic
{
    class CCAPIRequest
    {
        public string CheckMeeting(string apiKey, string RestKey, string userID)
        {
            try
            {
                return AsyncRequest(GetEventJson(), "events", "DELETE", apiKey, RestKey, userID);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in CheckMeeting:- " + ex.Message);
                return "Invalid";
            }
        }
        private string AsyncRequest(string strEventsJson, string strAction, string strMethod, string apiKey, string RestKey, string userID)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.craniumcafe.com/rest/v1/" + strAction);
                request.Method = strMethod;
                string postData = strEventsJson;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                request.Headers.Add("auth-key", apiKey);
                request.Headers.Add("auth-token", GenerateAuthToken(RestKey, userID));

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

                if (responseFromServer == "Unknown API key!\n")
                {
                    return "Invalid API Details!";
                }
                else if (GetJsonValue(responseFromServer, "success") == "false")
                {
                    string message = GetJsonValue(responseFromServer, "message");
                    return message; // "Invalid API Details!";
                }
                else
                {
                    return "Sucess";
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in AsyncRequest " + ex.Message);
                return "Invalid API Secret and Key";
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
                Debug.DebugMessage(2, "Error in GetJsonValue " + ex.Message);
                return "false";
            }

        }

        public string GenerateAuthToken(string RestKey, string userID)
        {
            try
            {
                string timestamp = UnixTimeNow();
                Random rnd = new Random();
                string rand = rnd.Next(1000000000).ToString();
                // string secretSalt = "vofyp67m9i19m37qj33cuhkdpe3banif3qv77e5ma0g75235z94d6hqepi2q2d6fegq1xmmbcflz3j3aexyium9pg5tg5s75kk4o";
                string secretSalt = RestKey;

                string authToken = timestamp + ":" + rand + ":" + userID + ":" + CalculateSHA1(timestamp + ":" + rand + ":" + userID + ":" + secretSalt);
                return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authToken));
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GenerateAuthToken " + ex.Message);
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
                Debug.DebugMessage(2, "Error in UnixTimeNow " + ex.Message);
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
                Debug.DebugMessage(2, "Error in CalculateSHA1 " + ex.Message);
                return "";
            }

        }

        public string GetEventJson()
        {
            try
            {
                string strEventsJson = "{\"event_guid\":\"" + 000 + "\"," +
                    "\"start_time\": \"" + DateTime.UtcNow.ToString("o") + "\", " +
                    "\"end_time\": \"" + DateTime.UtcNow.ToString("o") + "\", " +
                    "\"all_day_event\": " + "1";
                strEventsJson += ", \"subject\": \"" + "" + "\"}";


                return strEventsJson;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetEventJson:- " + ex.Message);
                return "";
            }
        }
    }
}
