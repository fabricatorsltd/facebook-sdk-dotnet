/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

#nullable enable
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using fabricators.Facebook.SDK.Models;
using Newtonsoft.Json;

namespace fabricators.Facebook.SDK
{
    public class Client
    {
        public string APIEndpoint => "https://graph.facebook.com/";
        internal string? AccessToken { get; set; }
        internal string? ClientSecret { get; }
        internal ulong? ClientId { get; }

        public Client(
            string accessToken)
        {
            if (!accessToken.StartsWith("&access_token="))
            {
                accessToken = "&access_token=" + accessToken;
            }
            AccessToken = accessToken;
        }
        
        public Client(
            ulong clientId,
            string? clientSecret,
            string? accessToken = null)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;

            if (accessToken == null) return;
            if (!accessToken.StartsWith("&access_token="))
            {
                accessToken = "&access_token=" + accessToken;
            }

            AccessToken = accessToken;
        }

        public IGraphResponse DebugToken()
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
                return JsonConvert.DeserializeObject<TokenData>(json).Data;
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ErrorRootObj>(responseText).Error;
            }
            catch (Exception ex)
            {
                dynamic error = new ExpandoObject();
                error.Message = ex.Message;
                return new Error
                {
                    Message = ex.Message
                };
            }
        }

        public IGraphResponse DoRequest(string url, Type returnObj)
        {
            if (returnObj.GetInterface("IGraphResponse") == null)
            {
                return null;
            }

            var webClient = new WebClient();

            try
            {
                var json = webClient.DownloadString(url);
                return (IGraphResponse)JsonConvert.DeserializeObject(json, returnObj);
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ErrorRootObj>(responseText).Error;
            }
            catch (Exception ex)
            {
                dynamic error = new ExpandoObject();
                error.Message = ex.Message;
                return new Error
                {
                    Code = -1,
                    Message = ex.Message
                };
            }
        }

        public IGraphPagedResponse DoPagedRequest(string url, Type returnObj)
        {
            if (returnObj.GetInterface("IGraphPagedResponse") == null)
            {
                return null;
            }

            WebClient webClient = new WebClient();

            try
            {
                var json = webClient.DownloadString(url);
                return (IGraphPagedResponse)JsonConvert.DeserializeObject(json, returnObj);
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ErrorRootObj>(responseText).Error;
            }
            catch (Exception ex)
            {
                dynamic error = new ExpandoObject();
                error.Message = ex.Message;
                return new Error
                {
                    Code = -1,
                    Message = ex.Message
                };
            }
        }
    }
}
