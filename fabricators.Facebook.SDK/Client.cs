/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

using System;
using System.Dynamic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace fabricators.Facebook.SDK
{
    public class Client
    {
        public string APIEndpoint => "https://graph.facebook.com/";
        public string AccessToken { get; internal set; }

        public Client(
            string accessToken)
        {
            if (!accessToken.StartsWith("&access_token="))
            {
                accessToken = "&access_token=" + accessToken;
            }
            AccessToken = accessToken;
        }

        public Models.IGraphResponse DebugToken()
        {
            WebClient client = new WebClient();
            string tempToken = AccessToken;
            if (tempToken.StartsWith("&access_token="))
            {
                tempToken = tempToken.Remove(0, 14);
            }

            try
            {
                var json = client.DownloadString(APIEndpoint  +
                                                 $"debug_token?input_token={tempToken}{AccessToken}");
                return JsonConvert.DeserializeObject<Models.TokenData>(json).Data;
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
                return new Models.Error
                {
                    Message = ex.Message
                };
            }
        }

        public Models.IGraphResponse DoRequest(string url, Type returnObj)
        {
            if (returnObj.GetInterface("IGraphResponse") == null)
            {
                return null;
            }

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
                return new Models.Error
                {
                    Code = -1,
                    Message = ex.Message
                };
            }
        }

        public Models.IGraphPagedResponse DoPagedRequest(string url, Type returnObj)
        {
            if (returnObj.GetInterface("IGraphPagedResponse") == null)
            {
                return null;
            }

            WebClient webClient = new WebClient();

            try
            {
                var json = webClient.DownloadString(url);
                return (Models.IGraphPagedResponse)JsonConvert.DeserializeObject(json, returnObj);
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
                return new Models.Error
                {
                    Code = -1,
                    Message = ex.Message
                };
            }
        }
    }
}
