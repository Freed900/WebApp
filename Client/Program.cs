using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static int port = 5000;
        static string address = "127.0.0.1";
        
        static void Main(string[] args)
        {
            do
            {

                string number;
                int value;
                do
                {
                    Console.Clear();
                    Console.Write("Введите число:");
                    number = Console.ReadLine();

                }
                while (!int.TryParse(number, out value));
                string[] words = GetWordsFromServer(value);
                foreach (var word in words)
                    Console.Write(word + " ");
                Console.WriteLine("\nДля выхода нажмите ESC");

            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static string[] GetWordsFromServer(int count)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);


                byte[] data = BitConverter.GetBytes(count);
                socket.Send(data);

                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return builder.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new string[0];
        }
    }
}
