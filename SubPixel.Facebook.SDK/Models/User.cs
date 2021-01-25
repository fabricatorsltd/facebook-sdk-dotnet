/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SubPixel.Facebook.SDK.Models
{
    public class User : IGraphResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("admin_notes")]
        public string AdminNotes { get; set; } // list<PageAdminNote>
        
        [JsonProperty("age_range")]
        public string AgeRange { get; set; }

        [JsonProperty("auth_method")]
        public string AuthMethod { get; set; } // enum?

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("can_review_measurement_request")]
        public bool CanReviewMeasurementRequest { get; set; }

        [JsonProperty("education")]
        public string Education { get; set; } // list<EducationExperience>

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("favorite_athletes")]
        public string FavoriteAthletes { get; set; } // list<Experience>

        [JsonProperty("favorite_teams")]
        public string FavoriteTeams { get; set; } // list<Experience>

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("hometown")]
        public string Hometown { get; set; } // Page

        [JsonProperty("inspirational_people")]
        public string InspirationalPeople { get; set; } // list<Experience>

        [JsonProperty("install_type")]
        public string InstallType { get; set; } // enum?

        [JsonProperty("installed")]
        public bool Installed { get; set; }

        [JsonProperty("is_famedeeplinkinguser")]
        public string IsFAMEDeeplinkingUser { get; set; }

        [JsonProperty("is_guest_user")]
        public bool IsGuestUser { get; set; }

        [JsonProperty("is_shared_login")]
        public bool IsSharedLogin { get; set; }

        [JsonProperty("languages")]
        public string Languages { get; set; } // list<Experience>

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; } // Page

        [JsonProperty("meeting_for")]
        public string MeetingFor { get; set; } // list<string>

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_format")]
        public string NameFormat { get; set; }

        [JsonProperty("payment_pricepoints")]
        public string PaymentPricepoints { get; set; } // PaymentPricepoints

        [JsonProperty("profile_pic")]
        public string ProfilePic { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("quotes")]
        public string Quotes { get; set; }

        [JsonProperty("security_settings")]
        public string SecuritySettings { get; set; } // SecuritySettings

        [JsonProperty("shared_login_upgrade_required_by")]
        public DateTime SharedLoginUpgradeRequiredBy { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("significant_other")]
        public string SignificantOther { get; set; } // User

        [JsonProperty("sports")]
        public string Sports { get; set; } // list<Experience>

        [JsonProperty("supports_donate_button_in_live_video")]
        public bool SupportsDonateButtonInLiveVideo { get; set; }

        [JsonProperty("test_group")]
        public uint TestGroup { get; set; }

        [JsonProperty("token_for_business")]
        public string TokenForBusiness { get; set; }

        [JsonProperty("video_upload_limits")]
        public string VideoUploadLimits { get; set; } // VideoUploadLimits

        [JsonProperty("viewer_can_send_gift")]
        public bool ViewerCanSendGift { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("work")]
        public string Work { get; set; } // list<WorkExperience>

        public enum Scope
        {
            id, first_name, last_name, middle_name, name, name_format,
            picture, short_name, email, birthday, gender, user_friends
        }

        public static IGraphResponse Me(Client client, List<Scope> scopes) => Get(client, 0, scopes);
        public static IGraphResponse Get(Client client, long userId, List<Scope> scopes,
            string before = null, string after = null)
        {
            string url = client.APIEndpoint;
            url += $"{(userId == 0 ? "me" : userId.ToString())}/?fields=";
            var _scopes = new StringBuilder();
            if (scopes != null)
            {
                foreach (Scope scope in scopes)
                {
                    _scopes.Append("," + scope);
                }
                _scopes = _scopes.Remove(0, 1);
            }
            else
            {
                _scopes = new StringBuilder("id,first_name,last_name,middle_name,name,name_format,picture,short_name");
            }

            string cursor = "";
            if (!string.IsNullOrEmpty(cursor)) cursor += $"&before={before}";
            if (!string.IsNullOrEmpty(after)) cursor += $"&after={after}";

            url += _scopes + cursor + client.AccessToken;

            return client.DoRequest(url, typeof(User));
        }
    }
}