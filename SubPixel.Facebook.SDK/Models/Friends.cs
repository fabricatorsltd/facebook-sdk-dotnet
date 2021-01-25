/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace SubPixel.Facebook.SDK.Models
{
    public class Friends : IGraphPagedResponse
    {
        [JsonProperty("data")]
        public List<User> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("summary")]
        public PagedResponseSummary Summary { get; set; }

        public static IGraphPagedResponse Get(Client client, long userId,
            string before = null, string after = null)
        {
            string url = client.APIEndpoint;
            url += $"{(userId == 0 ? "me" : userId.ToString())}/friends?";

            string cursor = "";
            if (!string.IsNullOrEmpty(cursor)) cursor += $"&before={before}";
            if (!string.IsNullOrEmpty(after)) cursor += $"&after={after}";

            url += cursor + client.AccessToken;

            return client.DoPagedRequest(url, typeof(Friends));
        }
    }
}
