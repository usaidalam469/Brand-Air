using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Drawing;

namespace Brand_Air
{
    class ClientSocket
    {
        //private static readonly Socket CSocket = new Socket
        //    (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ////private static int HeaderSize=10;

        Socket CSocket;
        private const int PORT = 12347;
        public ClientSocket()
        {
            CSocket = new Socket
           (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public bool ConnectToServer()
        {
            //int attempts = 0;
            if (CSocket == null)
            {
                CSocket = new Socket
               (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            while (!CSocket.Connected)
            {
                try
                {
                    CSocket.Connect("192.168.18.7", PORT);

                }
                catch (SocketException)
                {
                    return false;
                }
            }
            return true;

        }

        public void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            CSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        public void SendBytes(byte[] buffer)
        {
            CSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        public string ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = CSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return "";
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            return text;
        }
        public byte[] ReceiveImage()
        {
            var buffer = new byte[90000];
            int received = CSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            //string text = Encoding.ASCII.GetString(data);
            return data;
        }
        public byte[] Receive()
        {
            var buffer = new byte[10];
            int received =  CSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            string text = Encoding.ASCII.GetString(buffer);
            buffer = new byte[int.Parse(text.Split()[0])];
            received = CSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            return buffer;
        }
        public void Exit()
        {
            //CSocket.Shutdown(SocketShutdown.Both);
            CSocket.Close();
            // Environment.Exit(0);
        }
    }

}
