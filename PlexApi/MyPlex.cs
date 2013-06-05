using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using RestSharp;
using System.Xml;

namespace PlexAPI
{

	public class Utils
	{

		public static DateTime GetDateTimeFromTimestamp (string timestamp)
		{
			// First make a System.DateTime equivalent to the UNIX Epoch.
			System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

			// Add the number of seconds in UNIX timestamp to be converted.
			return dateTime.AddSeconds(Double.Parse(timestamp));
		}
	}

	public abstract class PlexRest
	{
		const string BaseUrl = "https://my.plexapp.com";

		protected RestClient GetRestClient()
		{
			var client = new RestClient();
			client.BaseUrl = GetBaseUrl();
			return client;
		}

		protected virtual string GetBaseUrl()
		{
			return BaseUrl;
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

			request.AddParameter("X-Plex-Token", user.authenticationToken);

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

			request.AddParameter("X-Plex-Token", user.authenticationToken);

			return Execute(request, client);
		}

		public String Execute(RestRequest request, String username, String password)
		{

			var client = GetRestClient();

			request = AddPlexHeaders(request);

			client.Authenticator = new HttpBasicAuthenticator(username, password);

			return Execute(request, client);
		}
	}
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
		public DateTime joinDate { get; set; }
	}

	public class Server : PlexRest
	{
		public string accessToken { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public Int32 port { get; set; }
		public string version { get; set; }
		public string host { get; set; }
		public string localAddresses { get; set; }
		public string machineIdentifier { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public bool owned { get; set; }
		public User user { get; set; }

		public Server (User user)
		{
			this.user = user;
		}

		protected override string GetBaseUrl()
		{
			return "http://" + address + ":" + port;
		}

		public List<Section> GetLibrarySections()
		{
			var request = new RestRequest();
			request.Resource = "library/sections";

			var xml = Execute(request, user);
			/*
			<MediaContainer size="2" allowSync="0" identifier="com.plexapp.plugins.library" mediaTagPrefix="/system/bundle/media/flags/" mediaTagVersion="1364961527" title1="Plex Library">
			<Directory art="/:/resources/movie-fanart.jpg" filters="1" refreshing="0" thumb="/:/resources/movie.png" key="2" type="movie" title="Movies" agent="com.plexapp.agents.imdb" scanner="Plex Movie Scanner" language="en" uuid="" updatedAt="1368592742" createdAt="1368588077">
			<Location path="<ABS PATH ON SERVER>"/>
			</Directory>
			<Directory art="/:/resources/show-fanart.jpg" filters="1" refreshing="0" thumb="/:/resources/show.png" key="1" type="show" title="TV Shows" agent="com.plexapp.agents.thetvdb" scanner="Plex Series Scanner" language="en" uuid="" updatedAt="1370397936" createdAt="1368588047">
			<Location path="<ABS PATH ON SERVER>"/>
			</Directory>
			</MediaContainer>
            */
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);

			XmlNodeList nodes = xmlDoc.GetElementsByTagName("Directory");

			List<Section> sections = new List<Section>();

			for (var i = 0; i < nodes.Count; i++)
			{
				Section section = new Section(this);
				XmlNode s = nodes.Item (i);

				section.art  = s.Attributes["art"].Value;
				section.filters  = s.Attributes["filters"].Value == "1";
				section.refreshing  = s.Attributes["refreshing"].Value == "1";
				section.thumb  = s.Attributes["thumb"].Value;
				section.key  = Int32.Parse (s.Attributes["key"].Value);
				section.type  = s.Attributes["type"].Value;
				section.title  = s.Attributes["title"].Value;
				section.agent  = s.Attributes["agent"].Value;
				section.scanner  = s.Attributes["scanner"].Value;
				section.language  = s.Attributes["language"].Value;
				section.uuid  = s.Attributes["uuid"].Value;
				section.createdAt  = Utils.GetDateTimeFromTimestamp(s.Attributes["createdAt"].Value);
				section.updatedAt  = Utils.GetDateTimeFromTimestamp(s.Attributes["updatedAt"].Value);

				sections.Add(section);
			}

			return sections;
		}
	}

	public class Section : PlexRest
	{
		public string art { get; set; }
		public bool filters { get; set; }
		public bool refreshing { get; set; }
		public string thumb { get; set; }
		public Int32 key { get; set; }
		public string type { get; set; }
		public string title { get; set; }
		public string agent { get; set; }
		public string scanner { get; set; }
		public string language { get; set; }
		public string uuid { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }

		public Server server { get; set; }
		public User user { get; set; }

		public Section (Server server)
		{
			this.server = server;
			this.user = server.user;
		}

		protected override string GetBaseUrl()
		{
			return "http://" + server.address + ":" + server.port;
		}
	}

    public class MyPlex : PlexRest
    {

        public User Authenticate(string username, string password)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "users/sign_in.xml";

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
				//user.joinDate = DateTime.Parse(xmlDoc.GetElementsByTagName ("joined-at").Item (0).InnerText);
            }

            return user;
		}

		public void Pms(User user)
		{
			var request = new RestRequest(Method.GET);
			request.Resource = "pms";

			var xml = Execute(request, user);
			/*
			<MediaContainer friendlyName="myPlex" machineIdentifier="<Other Hash>" identifier="com.plexapp.plugins.myplex" size="3">
			   <Directory key="library" title="library"/>
			   <Directory key="servers" title="servers"/>
			   <Directory key="system" title="system"/>
			</MediaContainer>
			*/
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml(xml);
			//XmlNodeList container = xmlDoc.GetElementsByTagName("MediaConainer");
		}

		public List<Server> GetServers(User user)
		{
			var request = new RestRequest(Method.GET);
			request.Resource = "pms/servers";

			var xml = Execute(request, user);
			/*
			<MediaContainer friendlyName="myPlex" identifier="com.plexapp.plugins.myplex" machineIdentifier="<HASH>" size="1">
				<Server accessToken="<AUTHTOKEN>" name="<NAME OF SERVER>" address="<IP ADDRESS>" port="<PORT>" version="<SERVER VERSION>" host="<HOST/IP>" localAddresses="<LOCAL IP>" machineIdentifier="<HASH>" createdAt="<UNIX TIMESTAMP>" updatedAt="<UNIX TIMESTAMP>" owned="1"/>
			</MediaContainer>
			*/
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml(xml);
			XmlNodeList server_nodes = xmlDoc.GetElementsByTagName("Server");

			List<Server> servers = new List<Server>();

			for (var i = 0; i < server_nodes.Count; i++)
			{
				Server server = new Server(user);
				XmlNode s = server_nodes.Item (i);
				
				server.accessToken  = s.Attributes["accessToken"].Value;
				server.name  = s.Attributes["name"].Value;
				server.address  = s.Attributes["address"].Value;
				server.port  = Int32.Parse(s.Attributes["port"].Value);
				server.version  = s.Attributes["version"].Value;
				server.host  = s.Attributes["host"].Value;
				server.localAddresses  = s.Attributes["localAddresses"].Value;
				server.machineIdentifier  = s.Attributes["machineIdentifier"].Value;
				server.createdAt  = Utils.GetDateTimeFromTimestamp(s.Attributes["createdAt"].Value);
				server.updatedAt  = Utils.GetDateTimeFromTimestamp(s.Attributes["updatedAt"].Value);
				server.owned  = s.Attributes["owned"].Value == "1";

				servers.Add(server);
			}

			return servers;

		}

    }
}
