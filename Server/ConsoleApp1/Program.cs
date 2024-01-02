using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program Server = new Program();
            Server.Create();

        }

        private void Create()
        {
            SocketServer Server = new SocketServer();
            new Thread(new ThreadStart(() =>
            {
                Server.AcceptClients();
            })).Start();

            Server.OnClientConnectedEvent += NewConnection;
        }

        private void NewConnection(Socket ClientSocket)
        {
            //new connection
            Console.WriteLine("[+] New connection: " + ClientSocket.RemoteEndPoint.ToString());
            Client ClientInstance = new Client(ClientSocket);
            ClientInstance.Read();
            Console.WriteLine("[-] Lost connection: " + ClientSocket.RemoteEndPoint.ToString());
            ClientSocket.Close();
        }
    }
}