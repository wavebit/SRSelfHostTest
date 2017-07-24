using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SRSelfHostTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set connection
            var connection = new HubConnection($"http://127.0.0.1:8081");
            ServicePointManager.DefaultConnectionLimit = 500;
            //Make proxy to hub based on hub name on server
            IHubProxy nhubProxy = connection.CreateHubProxy("Actor");

            //Start connection

            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                        task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();

            //for (int i = 0; i < 1; ++i)
            {
                Parallel.For(0, 8, (n) =>
                    {
                        var st = new Stopwatch();
                        st.Start();

                        var r = nhubProxy.Invoke<float>("Move", new object[] { 10.3f, 102.0f });

                        Console.WriteLine($"Current thread {Thread.CurrentThread.ManagedThreadId}");
                        r.Wait();

                        Console.WriteLine($"{n} Elapsed {st.ElapsedMilliseconds} ms");
                    }
                );
            }

            connection.Stop();

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
