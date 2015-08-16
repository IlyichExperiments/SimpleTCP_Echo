using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace Client
{
    class Client
    {
        const int ECHO_PORT = 8082;
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя:");
            string userName = Console.ReadLine();
            try
            {
                TcpClient eClient = new TcpClient("127.0.0.1", ECHO_PORT);

                StreamReader readerStream = new StreamReader(eClient.GetStream());
                NetworkStream writerStream = eClient.GetStream();

                string dataToSend = userName + "\r\n";
                byte[] data = Encoding.UTF8.GetBytes(dataToSend);
                writerStream.Write(data, 0, data.Length);

                while (true)
                {
                    Console.Write("{0}:", userName);
                    dataToSend = Console.ReadLine();
                    dataToSend += "\r\n";
                    data = Encoding.UTF8.GetBytes(dataToSend);
                    writerStream.Write(data, 0, data.Length);
                    if (dataToSend.IndexOf("QUIT") > -1) break;
                    string returnData;
                    returnData = readerStream.ReadLine();
                    Console.WriteLine("Server: {0}", returnData);

                }
                eClient.Close();


            }// try
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.GetType());
                
            }

        }
    }
}
