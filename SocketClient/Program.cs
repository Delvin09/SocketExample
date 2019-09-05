using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port)
        {
            // Create remote endpoint and socket for remote connection
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, port);
            Socket sender = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            Console.WriteLine("Socked was connect to {0} ", sender.RemoteEndPoint.ToString());

            Console.Write("Enter a message: ");
            string message = Console.ReadLine();
            byte[] msg = Encoding.UTF8.GetBytes(message);
            int bytesSent = sender.Send(msg); // send data

            // Recive responce
            byte[] bytes = new byte[1024];
            int bytesRec = sender.Receive(bytes);

            Console.WriteLine("\nServer responce: {0}\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            if (message.IndexOf("<cmd_end>") == -1)
                SendMessageFromSocket(port);

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
