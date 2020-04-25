using SubPixel.Facebook.SDK;
using SubPixel.Facebook.SDK.Models;
using System;
using System.Collections.Generic;

namespace SubPixel.Facebook.Tester
{
    class Program
    {
        static Client fbClient;
        const string AUTH_TOKEN = "EAALYkiORC4QBAN1LNKw7bBUj4zickORQ1thuVt6xhP97pkBWWVP84ZCyFjFVYrqODiiYK71VscjixbkN2VlXwKcvgZB6U2ZBvozKaoNqYlugKi9EIHopC2BeMRIX3DHhjV6yZCFqCxaKZAjrAszGh5hhLs8JeoFw9aPDa0x1QpRVv1YLoZCmf7200r8oSRIgIbXwWXJ8ZBf2QZDZD";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            fbClient = new Client(AUTH_TOKEN);

            string[] fbScopes = Scopes(fbClient);
            List<User.Scope> userScopes = new List<User.Scope>();
            if (fbScopes != null)
            {
                Console.WriteLine("\nScopes list\n");
                foreach (string scope in fbScopes)
                {
                    Console.WriteLine("\t" + scope);

                    Enum.TryParse(typeof(User.Scope), scope, out object _scope);
                    if (_scope != null)
                        userScopes.Add((User.Scope)_scope);

                    if(scope == "public_profile")
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
                Console.WriteLine("\n --");
            }

            GetMe(fbClient, userScopes);
        }

        static string[] Scopes(Client client)
        {
            var response = client.DebugToken();
            if (response.GetType() == typeof(DebugToken))
            {
                DebugToken token = (DebugToken)response;
                return token.Scopes;
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
            List<User.Scope> _scopes = new List<User.Scope>();
            _scopes.Add(User.Scope.id);
            _scopes.Add(User.Scope.name);

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
    }
}
