
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace Server
{
    class Program
    {
        static int port = 5000;

        static string[] words = {"Гусь", "Утка", "Трава", "Вор", "Ябеда", "Свинья", "Лебедь", "Птичка", "Книга"}; 

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                    }
                    while (handler.Available > 0);
                    byte[] newArr = new byte[bytes];
                    Array.Copy(data, newArr, bytes);

                    int value= BitConverter.ToInt32(newArr);
                    if(value>=0 && value<words.Length)
                    {
                        StringBuilder wordsToSend = new StringBuilder();
                        Random random = new Random();
                        for (int i = 0; i < value; i++)
                            wordsToSend.Append( words[random.Next(0,words.Length)]+",");
                        handler.Send(Encoding.Unicode.GetBytes(wordsToSend.ToString()));

                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " +value);
                    }
                    else
                    {
                        handler.Send(Encoding.Unicode.GetBytes("parameter is not an number"));
                    }
              
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
