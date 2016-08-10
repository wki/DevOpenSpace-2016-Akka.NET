using System;
using Microsoft.Owin.Hosting;
using System.Threading;

namespace Frontend
{
    class Program
    {
        // netsh http add urlacl url=http://localhost:9000/ user=everyone
        const int Port = 9000;

        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: String.Format("http://localhost:{0}/", Port)))
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

                Console.WriteLine("Listening on Port {0}", Port);
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}

