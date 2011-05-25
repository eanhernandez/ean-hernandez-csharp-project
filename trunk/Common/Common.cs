using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace Common
{
    public static class CommsTools
    {
        public static void SendMCastData(String s, Socket mdpSocket, IPEndPoint mcastEp)
        {
            byte[] sendBuffer = new byte[512];
            sendBuffer = Encoding.ASCII.GetBytes(s);
            mdpSocket.SendTo(sendBuffer, mcastEp);
        }
        public static Socket SetUpMCastSendSocket()
        {
            Socket mdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 3);
            return mdpSocket;
        }
        public static Socket SetUpMcastListenSocket(int port)
        {
            IPHostEntry entry = Dns.GetHostByName(Dns.GetHostName());
            EndPoint localEp = new IPEndPoint(entry.AddressList[0], port);
            Socket mdcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdcSocket.Bind(localEp);
            IPAddress groupAddress = IPAddress.Parse("224.5.6.7");
            MulticastOption mcastOption = new MulticastOption(groupAddress);
            mdcSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            return mdcSocket;
        }
    }
    public static class Tools
    {
        public static void ValidateOrderRequest(string s)
        {
            // MSFT,Regular,B,22.82,1
            string[] vals = s.Split(',');

            if  (
                (Regex.IsMatch(vals[0], @"[^A-Z]"))             // symbol is not letters only
            ||  (!vals[1].Equals("Regular"))                    // type is not "Regular"
            ||  (Regex.IsMatch(vals[2], @"[^BS]"))              // order type is not B or S
            ||  (!Regex.IsMatch(vals[3], @"^[0-9]*[.]?[0-9]+$"))// price is not a float
            ||  (!Regex.IsMatch(vals[4], @"^[0-9]"))            // quantity is not numbers
                )
            {
                throw new BadOrderInput();
            }
            else
            {
                Console.WriteLine("REGEX OK");
            }
        }
    }

}
