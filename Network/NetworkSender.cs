using blockchain_prototype.Network.Structures;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace blockchain_prototype.Network
{
    internal class NetworkSender : ISendable
    {
        public void SendData()
        {
            throw new NotImplementedException();
        }

        public void Test()
        {
            int port = 5555;
            string ipAddress = "192.168.0.100";
            OpenPort(port);
            Task.Run(() => SendData(ipAddress, port, "Test!"));
            Console.ReadLine();
        }

        private static void OpenPort(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Порт {port} открыт");

            Task.Run(async () =>
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    Task.Run(() =>
                    {
                        HandleClient(client);
                        client.Close();
                    });
                }
            });
        }

        private static void HandleClient(TcpClient client)
        {
            if (client == null)
            {
                throw new Exception("Client is null");
                return;
            }

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Получено: {message}");
        }

        private static void SendData(string ipAddress, int port, string data)
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            {
                NetworkStream stream = client.GetStream();

                byte[] buffer = Encoding.UTF8.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Отправлено");
            }
        }

        private static void SendPacket(string ipAddress, int port, Packet packet)
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            {
                NetworkStream stream = client.GetStream();

                byte[] buffer = Serialize(packet);
                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Отправлено");
            }
        }

        private static byte[] Serialize(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        private static Packet Deserialize(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                return (Packet)formatter.Deserialize(memoryStream);
            }
        }
    }

}
