using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using PlexAPI;

namespace Test
{
    class Program
    {

        static MyPlex plex;
		static User user;
		static Server server;
		static Directory section;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello from C#.");
            plex = new MyPlex();

            var command = "";
            do
            {
                Console.WriteLine("Enter a command");
                command = Console.ReadLine();
                Console.WriteLine("Running " + command);
                switch (command)
                {
                case "auth":
                case "authenticate":
                    Authenticate();
                    break;
				case "atoken":
					AToken();
				break;
				case "servers":
					Servers();
					break;
				case "sections":
					Sections();
					break;
				case "views":
					Views ();
					break;
				case "traverse":
					Traverse ();
					break;
                }
            } while (command != "quit");

        }

        static void Authenticate()
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            user = plex.Authenticate(username, password);
			Console.WriteLine("Plex Auth Token: " + user.authenticationToken);
        }

		static void AToken()
		{
			user = new User();
			Console.Write ("Enter Plex Auth Token: ");
			user.authenticationToken = Console.ReadLine();
		}

		static void Servers()
		{
			List<Server> servers = plex.GetServers(user);
			int i;

			for (i = 0; i < servers.Count; i++) {
				Console.WriteLine ("Found Server [" + i + "]: " + servers[i].name);
			}

			Console.Write ("Select Server Number: ");
			i = int.Parse (Console.ReadLine ());
			server = servers[i];
		}

		static void Sections()
		{
			Console.WriteLine ("Looking for library sections on " + server.name);
			List<Directory> sections = server.GetLibrarySections ();
			for (var j = 0; j < sections.Count; j++) {
				Console.WriteLine ("Found Section [" + j + "]: " + sections[j].title);
				Console.WriteLine ("Type: " + sections[j].type);
			}

			Console.Write ("Select Section Number: ");
			int i = int.Parse (Console.ReadLine ());
			section = sections[i];
		}

		static void Views()
		{
			/*Console.WriteLine ("Looking for views on " + section.title);
			List<PlexAPI.Section.View.Directory> views = section.GetViews ();
			for (var j = 0; j < views.Count; j++) {
				Console.WriteLine ("Found View [" + j + "]: " + views[j].title);
				Console.WriteLine ("Searchable: " + views[j].search.ToString());
			}

			Console.Write ("Select View Number: ");
			int i = int.Parse (Console.ReadLine ());*/
			//section = sections[i];
		}

		static void Traverse()
		{
			int command = -1;
			string uri = "library/sections/";
			do {
				var mc = new MediaContainer (user, server, uri);
				for (var i = 0; i < mc.videos.Count; i++) {
					Console.WriteLine ("[" + i + "]" + mc.videos[i].title + " -- " + mc.videos[i].uri);
				}
				for (var i = 0; i < mc.directories.Count; i++) {
					Console.WriteLine ("[" + i + "]" + mc.directories[i].title + " -- " + mc.directories[i].uri);
				}
				Console.Write("Which directory? ");
				command = int.Parse (Console.ReadLine());
				uri = mc.directories[command].uri;
			} while (command >= 0);

		}

    }
}
