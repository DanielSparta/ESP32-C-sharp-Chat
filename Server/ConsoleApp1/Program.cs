using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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

            string ipaddress = CheckConfigExist();
            SocketServer Server = new SocketServer(ipaddress);
            new Thread(new ThreadStart(() =>
            {
                a:
                bool ErrorReturned = Server.AcceptClients();
                //Error returned, create server again from other ipaddress
                ipaddress = CheckConfigExist();
                Server = new SocketServer(ipaddress);
                goto a;
            })).Start();

            Server.OnClientConnectedEvent += NewConnection;
        }


        private String CheckConfigExist()
        {
            string ipaddress = "";
            string FilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ipaddress.txt";

            if (!File.Exists(FilePath))
            {
                Console.WriteLine("[i] - No saved config detected. \r\n");
            a:
                Console.Write("Please enter your ipv4 IpAddress: ");
                try
                {
                    IPAddress ip = IPAddress.Parse(Console.ReadLine());
                    ipaddress = ip.ToString();
                }
                catch
                {
                    Console.WriteLine("[ERROR] - invalid ipaddress.");
                    Console.WriteLine(@"[ERROR] - IpAddress """ + ipaddress + @""" is not in valid context. it need to be taken from ""ipconfig"" command at cmd");
                    goto a;
                }

                File.Create(FilePath).Close();
                File.WriteAllText(FilePath, ipaddress);
                Console.WriteLine(@"Writing ipaddress to """ + FilePath + @"""" + "\n\n\n");
                return ipaddress;
            }
            else
            {
                ipaddress = File.ReadAllText(FilePath);
                Console.WriteLine("[i] - Detected ipaddress saved config");
                Console.WriteLine(@"[i] - Using default config path: """ + FilePath + @"""");
                Console.WriteLine("[i] - Using " + ipaddress + " as default ipaddress.");
                Console.WriteLine(@"[i] - If you wish to change the IpAddress, You need to open the saved text file from path and change its ipaddress." + "\n\n\n");
                return ipaddress;
            }
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