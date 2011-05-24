using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Common
{
    public static class CommsTools
    {
        public static void SendMCastData(String s, Socket mdpSocket, IPEndPoint mcastEP)
        {
            byte[] sendBuffer = new byte[512];
            sendBuffer = Encoding.ASCII.GetBytes(s);
            mdpSocket.SendTo(sendBuffer, mcastEP);
        }
        public static Socket SetUpMCastSendSocket()
        {
            Socket mdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 3);
            return mdpSocket;
        }
        public static Socket setUpMcastListenSocket(int port)
        {
            IPHostEntry entry = Dns.GetHostByName(Dns.GetHostName());
            EndPoint localEP = new IPEndPoint(entry.AddressList[0], port);
            Socket mdcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdcSocket.Bind(localEP);
            IPAddress groupAddress = IPAddress.Parse("224.5.6.7");
            MulticastOption mcastOption = new MulticastOption(groupAddress);
            mdcSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            return mdcSocket;
        }
    }
}
