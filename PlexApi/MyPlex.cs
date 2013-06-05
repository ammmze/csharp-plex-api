using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace PlexAPI
{
    public class User
    {
        public string username { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string thumb { get; set; }
        public string queueEmail { get; set; }
        public string queueUid { get; set; }
        public string cloudSyncDevice { get; set; }
        public string authenticationToken { get; set; }
    }
    public class MyPlex
    {
        const string BaseUrl = "https://my.plexapp.com";

        protected RestClient GetRestClient(String baseurl = BaseUrl)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            return client;
        }

        protected RestRequest AddPlexHeaders(RestRequest request)
        {
            request.AddHeader("X-Plex-Platform", "Windows");
            request.AddHeader("X-Plex-Platform-Version", "7");
            request.AddHeader("X-Plex-Provides", "player");
            request.AddHeader("X-Plex-Client-Identifier", "AB6CCCC7-5CF5-4523-826A-B969E0FFD8A0");
            request.AddHeader("X-Plex-Product", "PlexWMC");
            request.AddHeader("X-Plex-Version", "0");

            return request;
        }

        public T Execute<T>(RestRequest request, RestClient client) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            return response.Data;
        }

        public T Execute<T>(RestRequest request, User user) where T : new()
        {
            var client = GetRestClient();

            request = AddPlexHeaders(request);

            request.AddParameter("auth_token", user.authenticationToken);

            return Execute<T>(request, client);
        }

        public T Execute<T>(RestRequest request, String username, String password) where T : new()
        {

            var client = GetRestClient();

            request = AddPlexHeaders(request);
            
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            return Execute<T>(request, client);
        }

        public User authenticate(string username, string password)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl + "/users/sign_in.xml";
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(Method.POST);
            request.Resource = "users/sign_in.xml";
            request.RootElement = "user";

            var user = Execute<User>(request, username, password);

            return user;
        }

    }
}
