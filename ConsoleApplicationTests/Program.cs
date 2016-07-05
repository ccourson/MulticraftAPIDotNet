using MulticraftAPIDotNet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McServers = System.Collections.Generic.Dictionary<string, string>;

namespace ConsoleApplicationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            MulticraftAPI api = new MulticraftAPI()
            {
                url = "http://127.0.0.1:81/api.php",
                user = "admin",
                key = "c327PXiMAWNn+6",
                timeout = 3000
            };

            Console.WriteLine("FindServers");
            var result = api.FindServers( new[] { "players", "memory" }, new[] { "10", "1024" } );
            foreach (var item in result.data as McServers)
                Console.WriteLine("id: {0} name: {1}", item.Key, item.Value);

            Console.WriteLine("\nListServers");
            result = api.ListServers();
            foreach (var item in result.data as McServers)
                Console.WriteLine("id: {0} name: {1}", item.Key, item.Value);

            Console.WriteLine("\nListUsers");
            result = api.ListUsers();
            foreach (var item in result.data as McServers)
                Console.WriteLine("id: {0} name: {1}", item.Key, item.Value);

            // =====================

            Console.WriteLine("\nGetUser");
            result = api.GetUser(1);
            foreach (var item in result.data as McServers)
                Console.WriteLine("field: {0} value: {1}", item.Key, item.Value);

            Console.WriteLine("\nGetCurrentUser");
            result = api.GetCurrentUser();
            foreach (var item in result.data as McServers)
                Console.WriteLine("field: {0} value: {1}", item.Key, item.Value);
        }
    }
}
