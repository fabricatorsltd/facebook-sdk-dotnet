/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. 
 *
 * Author: Pietro di Caprio <pietro.dicaprio@subpixel.it>
 * Please open an issue on GitHub for any problem or question.
 */

using fabricators.Facebook.SDK;
using fabricators.Facebook.SDK.Models;
using System;
using System.Collections.Generic;

namespace fabricators.Facebook.Tester
{
    class Program
    {
        const string AUTH_TOKEN = "";

        static void Main(string[] args)
        {
            Console.WriteLine("fabricators Facebook SDK tester");
            Client fbClient = new Client(AUTH_TOKEN);

            //List<User.Scope> userScopes = Scopes(fbClient);
            //if(userScopes != null)
            //{
            //    if(userScopes.Count != 0)
            //    {
            //        GetMe(fbClient, userScopes);
            //    }
            //}

            GetMe(fbClient, null);
            GetFriends(fbClient);
        }

        static List<User.Scope> Scopes(Client client)
        {
            var response = client.DebugToken();
            if (response.GetType() == typeof(DebugToken))
            {
                DebugToken token = (DebugToken)response;
                if (token.Scopes != null)
                {
                    List<User.Scope> userScopes = new List<User.Scope>();
                    Console.WriteLine("\nScopes list\n");
                    foreach (string scope in token.Scopes)
                    {
                        Console.WriteLine("\t" + scope);

                        Enum.TryParse(typeof(User.Scope), scope.Replace("user_",""), out object _scope);
                        if (_scope != null)
                        {
                            userScopes.Add((User.Scope)_scope);
                        }

                        if (scope == "public_profile")
                        {
                            userScopes.Add(User.Scope.id);
                            userScopes.Add(User.Scope.first_name);
                            userScopes.Add(User.Scope.last_name);
                            userScopes.Add(User.Scope.middle_name);
                            userScopes.Add(User.Scope.name);
                            userScopes.Add(User.Scope.name_format);
                            userScopes.Add(User.Scope.picture);
                            userScopes.Add(User.Scope.short_name);
                        }
                    }
                    Console.WriteLine("\nEnd of scopes\n");
                    return userScopes;
                }
                return null;
            }
            else if (response.GetType() == typeof(Error))
            {
                Console.WriteLine(((Error)response).Message);
            }
            else
            {
                Console.WriteLine("Incorrect type:" + response.GetType());
            }

            return null;
        }

        static void GetMe(Client client, List<User.Scope> scopes)
        {
            var response = User.Me(client, scopes);

            if(response == null)
            {
                Console.WriteLine("response is null!");
                return;
            }

            if (response.GetType() == typeof(User))
            {
                User user = (User)response;
                Console.WriteLine(user.Name);
            }
            else if (response.GetType() == typeof(Error))
            {
                Console.WriteLine(((Error)response).Message);
            }
            else
            {
                Console.WriteLine("Incorrect type:" + response.GetType());
            }
        }

        static void GetFriends(Client client)
        {
            var response = Friends.Get(client, 0);

            if (response == null)
            {
                Console.WriteLine("response is null!");
                return;
            }

            if (response.GetType() == typeof(Friends))
            {
                var friends = (Friends)response;
                foreach (var friend in friends.Data)
                {
                    Console.WriteLine(friend.Name);
                }
            }
            else if (response.GetType() == typeof(Error))
            {
                Console.WriteLine(((Error)response).Message);
            }
            else
            {
                Console.WriteLine("Incorrect type:" + response.GetType());
            }
        }
    }
}
