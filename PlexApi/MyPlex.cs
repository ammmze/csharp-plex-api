using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Xml;

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

        public String Execute(RestRequest request, RestClient client)
        {
            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            return response.Content;
        }

        public String Execute(RestRequest request, User user)
        {
            var client = GetRestClient();

            request = AddPlexHeaders(request);

            request.AddParameter("auth_token", user.authenticationToken);

            return Execute(request, client);
        }

        public String Execute(RestRequest request, String username, String password)
        {

            var client = GetRestClient();

            request = AddPlexHeaders(request);

            client.Authenticator = new HttpBasicAuthenticator(username, password);

            return Execute(request, client);
        }

        public User authenticate(string username, string password)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl + "/users/sign_in.xml";
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(Method.POST);
            request.Resource = "users/sign_in.xml";
            request.RootElement = "user";

            var xml = Execute(request, username, password);
            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <user email="" id="" thumb="http://www.gravatar.com/avatar/HASH?d=404" username="" queueEmail="" queueUid="" cloudSyncDevice="" authenticationToken="">
                <subscription active="1" status="Active" plan="monthly">
                    <feature id="pass"/>
                    <feature id="sync"/>
                </subscription>
                <username></username>
                <email></email>
                <joined-at type="datetime">2001-01-01 00:00:00 UTC</joined-at>
                <authentication-token></authentication-token>
            </user>
            */
            XmlDocument xmlDoc = new XmlDocument(); //* create an xml document object.
            xmlDoc.LoadXml(xml); //* load the XML document from the specified file.

            //* Get elements.
            XmlNodeList user_nodes = xmlDoc.GetElementsByTagName("user");
            var user = new User();
            if (user_nodes.Count == 1) {
                XmlNode user_node = user_nodes.Item(0);
                user.email = user_node.Attributes["email"].Value;
                user.id = Int32.Parse(user_node.Attributes["id"].Value);
                user.thumb = user_node.Attributes["thumb"].Value;
                user.username = user_node.Attributes["username"].Value;
                user.queueEmail = user_node.Attributes["queueEmail"].Value;
                user.queueUid = user_node.Attributes["queueUid"].Value;
                user.cloudSyncDevice = user_node.Attributes["cloudSyncDevice"].Value;
                user.authenticationToken = user_node.Attributes["authenticationToken"].Value;
            }

            return user;
        }

    }
}
