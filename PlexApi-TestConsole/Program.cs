using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexAPI;

namespace Test
{
    class Program
    {

        static MyPlex plex;

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
                }
            } while (command != "quit");

        }

        static void Authenticate()
        {
            Console.WriteLine("Username");
            var username = Console.ReadLine();

            Console.WriteLine("Password");
            var password = Console.ReadLine();

            var user = plex.authenticate(username, password);
        }


    }
}
