using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Client
    {
        private Socket client;
        private bool Connected = true;
        public Client(Socket client)
        {
            this.client = client;
            new Thread(new ThreadStart(() => { Send(); })).Start();
        }

        public void Read()
        {
            new Thread(new ThreadStart(() => {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(10000);
                        this.client.Send(Encoding.ASCII.GetBytes(""));
                    }
                    catch
                    {
                        break;
                    }
                }
                return;
            })).Start();

            while (Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead > 0)
                        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                }
                catch (Exception e)
                {
                    this.Connected = false;
                    Console.WriteLine(e.Message.ToString());
                    break;
                }
            }
            return;
        }

        private void Send()
        {
            while (true)
            {

                try
                {
                    string message = Console.ReadLine();
                    this.client.Send(Encoding.ASCII.GetBytes(message));
                }catch
                {
                    this.Connected = false;
                    break;
                }
            }
            return;

        }
    }
}
