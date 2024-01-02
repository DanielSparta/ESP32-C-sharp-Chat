using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace ConsoleApp1
{
    public class SocketServer
    {   
        public delegate void ClientConnected(Socket ConnectedClient);
        public event ClientConnected OnClientConnectedEvent;
        private Socket socketserver;
        private bool SocketCreateError = false;

        public SocketServer(string ipaddress)
        {
            this.socketserver = server();
            IPEndPoint iep = SetIpDetail(ipaddress, 81);
            Console.WriteLine("[i] Server created: " + iep as string);
            try
            {
                socketserver.Bind(iep);
                socketserver.Listen(1000);
            }
            catch
            {
                Console.WriteLine(@"[ERROR] - IpAddress is not valid.");
                Console.WriteLine(@"[ERROR] - IpAddress """ + ipaddress + @""" is not in valid context. it need to be taken from ""ipconfig"" command at cmd");
                File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ipaddress.txt");
                this.SocketCreateError = true;
            }
        }




        private Socket server()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private IPEndPoint SetIpDetail(string ipaddress, int port)
        {
            return new IPEndPoint(IPAddress.Parse(ipaddress), port);
        }



        
        public bool AcceptClients()
        {
            if (SocketCreateError)
                return true;

            while (true)
            {
                Socket client = this.socketserver.Accept();
                OnClientConnectedEvent.Invoke(client);
            }
        }
    }
}
