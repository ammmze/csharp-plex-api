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
					case "pms":
						Pms();
						break;
					case "servers":
						Servers();
						break;
					case "sections":
						Sections();
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

		static void Pms()
		{
			plex.Pms(user);
		}

		static void Servers()
		{
			List<Server> servers = plex.GetServers(user);
			for (var i = 0; i < servers.Count; i++) {
				Console.WriteLine ("Found Server: " + servers[i].name);
			}
		}

		static void Sections()
		{
			List<Server> servers = plex.GetServers(user);
			
			for (var i = 0; i < servers.Count; i++) {
				Console.WriteLine ("Found Server: " + servers[i].name);
				List<Section> sections = servers [i].GetLibrarySections ();
				for (var j = 0; j < sections.Count; j++) {
					Console.WriteLine ("Found Section: " + sections[i].title);
					Console.WriteLine ("Type: " + sections[i].type);
				}
			}
		}

    }
}
