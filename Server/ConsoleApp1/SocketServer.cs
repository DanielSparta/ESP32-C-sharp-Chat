using System.Net.Sockets;
using System.Net;

namespace ConsoleApp1
{
    public class SocketServer
    {   
        public delegate void ClientConnected(Socket ConnectedClient);
        public event ClientConnected OnClientConnectedEvent;

        private Socket socketserver;

        public SocketServer()
        {
            this.socketserver = server();
            IPEndPoint iep = SetIpDetail("172.0.2.131", 81);
            Console.WriteLine("[i] Server created: " + iep as string);
            socketserver.Bind(iep);
            socketserver.Listen(1000);
        }




        private Socket server()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private IPEndPoint SetIpDetail(string ipaddress, int port)
        {
            return new IPEndPoint(IPAddress.Parse(ipaddress), port);
        }



        
        public void AcceptClients()
        {
            while(true)
            {
                Socket client = this.socketserver.Accept();
                OnClientConnectedEvent.Invoke(client);
            }
        }
    }
}
