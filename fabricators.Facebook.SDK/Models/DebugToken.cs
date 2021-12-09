/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

using Newtonsoft.Json;
using System.Collections.Generic;

namespace fabricators.Facebook.SDK.Models
{
    public class DebugToken : IGraphResponse
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public string DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("metadata")]
        public dynamic Metadata { get; set; }

        [JsonProperty("scopes")]
        public string[] Scopes { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("granular_scopes")]
        public IEnumerable<GranularScope> GranularScopes { get; set; }
    }

    public class GranularScope
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("target_ids")]
        public IEnumerable<long> TargetIds { get; set; }
    }

    public class TokenData : IGraphResponse
    {
        [JsonProperty("data")]
        public DebugToken Data { get; set; }
    }
}
