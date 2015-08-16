using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;


namespace SimpleTCP
{
    class ClientHandler
    {
        public TcpClient clientSocket;
        public void RunClient()
        {
            StreamReader readerStream = new StreamReader(clientSocket.GetStream());
            NetworkStream writerStream = clientSocket.GetStream();
            string returnData = readerStream.ReadLine();
            string userName = returnData;

            Console.WriteLine("Добро пожаловать, {0}!", userName);

            while (true)
            {
                returnData = readerStream.ReadLine();
                if (returnData.IndexOf("QUIT") > -1)
                {
                    Console.WriteLine("Пользователь {0} вышел", userName);
                    break;
                }

                Console.WriteLine("{0} : {1}", userName, returnData);
                returnData += "\r\n";
                byte[] dataWrite = Encoding.UTF8.GetBytes(returnData);
                writerStream.Write(dataWrite, 0, dataWrite.Length);
            } // while(true)
            clientSocket.Close();

        }
    }
    class Server
    {
        const int ECHO_PORT = 8082;
        public static int nClients = 0;

        static void Main(string[] args)
        {
            TcpListener clientListener = new TcpListener(ECHO_PORT);

            clientListener.Start();

            Console.WriteLine("Ожидаем подключения");

            while (true)
            {
                TcpClient client = clientListener.AcceptTcpClient();
                ClientHandler cHandler = new ClientHandler();
                cHandler.clientSocket = client;
                Thread clientThread = new Thread(new ThreadStart(cHandler.RunClient));
                clientThread.Start();
            }

            clientListener.Stop();
        }
    }
}
