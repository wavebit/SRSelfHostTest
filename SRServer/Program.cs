using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace SRServer
{
    class Program
    {

        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.

            string url = "http://127.0.0.1:8081";

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    [HubName("Actor")]
    public class ActorHub : Hub
    {
        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId} | {nameof(OnDisconnected)} ");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnConnected()
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId} | {nameof(OnConnected)} ");

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId} | {nameof(OnReconnected)} ");
            return base.OnReconnected();
        }

        public float Move(float x, float y)
        {
            return x + y;
        }
    }
}
