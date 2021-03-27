using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Operations
{
    class Operation
    {
        public static async Task WriteToLog(String text)
        {
            using StreamWriter file = new("log.txt", append: true);
            await file.WriteLineAsync(text + "\n");
        }
        public void Listen()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 6666;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();
                WriteToLog(Convert.ToString("Server started."));
                Byte[] bytes = new byte[256];
                String data = null;
                while (true)
                {
                    Console.Write("Waiting for a connection... \n");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!\n");
                    data = null;
                    int i, logged;
                    NetworkStream stream = client.GetStream();


                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        // Process the data sent by the client.
                        byte[] msg = Encoding.ASCII.GetBytes(data);
            
                        if (data == "TypicalUser 1234")
                        {
                            Console.WriteLine("Logged in with credentials: {0}\n", data);
                            WriteToLog(Convert.ToString("Logged in with credentials: {0}\n" + data));

                            msg = Encoding.ASCII.GetBytes("1");
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);
                            WriteToLog(Convert.ToString("Sent approval message to client.\n"));
                            logged = 1;
                        }
                        else
                        {
                            msg = Encoding.ASCII.GetBytes("0");
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Denied credentials: {0}", data);
                            WriteToLog(Convert.ToString(("Denied credentials: {0}", data)));
                            logged = 0;
                        }

                        if (logged == 1)
                        {
                            Console.Write("Awaiting orders from client...\n");

                        }
                    }
                client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
        }
    }
}