using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.Common;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.Sessions;

namespace SIS.MvcFramework
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly IServerRoutingTable serverRoutingTable;

        private readonly IHttpSessionStorage httpSessionStorage;

        private bool isRunning;

        public Server(int port, IServerRoutingTable serverRoutingTable, IHttpSessionStorage httpSessionStorage)
        {
            serverRoutingTable.ThrowIfNull(nameof(serverRoutingTable));
            httpSessionStorage.ThrowIfNull(nameof(httpSessionStorage));

            this.port = port;
            this.serverRoutingTable = serverRoutingTable;
            this.httpSessionStorage = httpSessionStorage;

            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), port);
        }

        private async Task ListenAsync(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable, this.httpSessionStorage);
            await connectionHandler.ProcessRequestAsync();
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                var client = this.tcpListener.AcceptSocketAsync().GetAwaiter().GetResult();
                Task.Run(() => this.ListenAsync(client));
            }
        }
    }
}
