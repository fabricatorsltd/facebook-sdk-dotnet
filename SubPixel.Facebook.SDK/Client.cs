using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SubPixel.Facebook.SDK
{
    public class Client
    {
        public string APIEndpoint { get { return "https://graph.facebook.com/"; } }
        public bool IsProduction { get; internal set; }
        public string AccessToken { get; internal set; }

        public Client(
            string accessToken)
        {
            if(!accessToken.StartsWith("&access_token="))
			    accessToken = "&access_token=" + accessToken;
            AccessToken = accessToken;
        }

        public Models.IGraphResponse DebugToken()
        {
            WebClient client = new WebClient();
            string tempToken = AccessToken;
            if (tempToken.StartsWith("&access_token="))
                tempToken = tempToken.Remove(0, 14);
            try
            {
                var json = client.DownloadString(APIEndpoint  +
                    String.Format("debug_token?input_token={0}&access_token={1}", tempToken, tempToken));
                return JsonConvert.DeserializeObject<Models.TokenData>(json).Data;
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<Models.Error>(responseText);
            }
            catch (Exception ex)
            {
                dynamic error = new ExpandoObject();
                error.Message = ex.Message;
                return new Models.Error()
                {
                    Message = ex.Message
                };
            }
        }

        public Models.IGraphResponse DoRequest(string url, Type returnObj)
        {
            if (returnObj.GetInterface("IGraphResponse") == null)
                return null;

            WebClient webClient = new WebClient();

            try
            {
                var json = webClient.DownloadString(url);
                return (Models.IGraphResponse)JsonConvert.DeserializeObject(json, returnObj);
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<Models.ErrorRootObj>(responseText).Error;
            }
            catch (Exception ex)
            {
                dynamic error = new ExpandoObject();
                error.Message = ex.Message;
                return new Models.Error()
                {
                    Code = -1,
                    Message = ex.Message
                };
            }
        }
    }
}
