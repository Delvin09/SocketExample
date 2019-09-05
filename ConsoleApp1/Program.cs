using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // Setup endpoint of server, bind soket to endpoint, and start listen.
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 11000);
                socket.Bind(ipEndPoint);
                socket.Listen(10);

                // Start get data form clients
                while (true)
                {
                    Console.WriteLine("Wait connection form: {0}", ipEndPoint);
                    Socket handler = socket.Accept();

                    // recive data from client to buffer
                    StringBuilder sb = new StringBuilder(1024);
                    byte[] bytes = new byte[1024];
                    int bytesRec = 0;
                    do
                    {
                        bytesRec = handler.Receive(bytes);
                        sb.Append(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                    }
                    while (bytesRec > 0 && bytesRec >= bytes.Length);

                    string data = sb.ToString();
                    Console.Write("Recived Message: " + data + "\n");

                    // Send reply|echo
                    string reply = "You send message of " + data.Length.ToString() + " symbols";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    try
                    {
                        if (data.IndexOf("<cmd_end>") > -1)
                        {
                            Console.WriteLine("Connection is closed.");
                            break;
                        }
                    }
                    finally
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
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
    }
}
